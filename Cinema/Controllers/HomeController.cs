using Cinema.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using Cinema.Entities;
using Cinema.Data;
using System.Diagnostics;

namespace Cinema.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CinemaDbContext context;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            context = new CinemaDbContext();
        }

        public async Task<IActionResult> Index([FromQuery] string? name, [FromQuery] string? genre)
        {
            IQueryable<Film> items = context.FilmTeams4;

            if (name != null)
                items = items.Where(x => x.Name.Contains(name));
            if (genre != null)
                items = items.Where(x => x.Genre.Contains(genre));
            return View(await items.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}