using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cinema.Data;
using System.Numerics;
using Cinema.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;

namespace Cinema.Controllers
{
    public class FilmsController : Controller
    {
        private readonly CinemaDbContext context;
        private readonly IEmailSender _emailSender;

        public FilmsController(CinemaDbContext context, IEmailSender emailSender)
        {
            this.context = context;
            _emailSender = emailSender;
        }

        public ActionResult Index()
        {
            var films = context.FilmTeams4
                .Include(x => x.Actors)
                .ToList();

            return View(films);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Film film)
        {
            if (!ModelState.IsValid)
                return View();

            context.FilmTeams4.Add(film);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var film = context.FilmTeams4.Find(id);
            if (film == null) return NotFound();

            LoadTeams();
            return View(film);
        }

        [HttpPost]
        public ActionResult Edit(Film film)
        {
            if (!ModelState.IsValid)
            {
                LoadTeams();
                return View();
            }

            context.FilmTeams4.Update(film);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var film = await context.FilmTeams4.FindAsync(id);

            if (film == null) return NotFound();

            await _emailSender.SendEmailAsync("danis_zs71@student.itstep.org", "Видалення команди",
                "<h1>Команда на видаленні</h1><p>" + film.Name + "</p>");

            return View(film);
        }

        public async Task<IActionResult> Details(int? id, string? returnUrl = null)
        {
            if (id == null) return NotFound();

            var film = await context.FilmTeams4.FindAsync(id);

            if (film == null) return NotFound(); 

            ViewBag.ReturnUrl = returnUrl;
            return View(film);
        }
        public ActionResult Delete(int id)
        {
            var film = context.FilmTeams4.Find(id);
            if (film == null) return NotFound();

            context.FilmTeams4.Remove(film);
            context.SaveChanges();

            return RedirectToAction("Index");
        }
        private void LoadTeams()
        {
            var teams = new SelectList(context.FilmTeams4.ToList(), "Id", "Name");
            ViewBag.Teams = teams;
        }








    }
}