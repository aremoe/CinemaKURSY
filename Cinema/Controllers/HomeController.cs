using Cinema.Models;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {
            return View(context.FilmTeams4.ToList()); 
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