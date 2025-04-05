using Cinema.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Controllers
{
    public class ActorsController : Controller
    {

        private readonly CinemaDbContext context;

        public ActorsController()
        {
            context = new CinemaDbContext();
        }

        // GET: 
        public ActionResult Index()
        {
            var actors = context.CinemaActors4
                .Include(x => x.Film)
                .ToList();

            return View(actors);
        }

        public ActionResult Delete(int id)
        {
            var actor = context.CinemaActors4.Find(id);
            if (actor == null) return NotFound();

            context.CinemaActors4.Remove(actor);
            context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
