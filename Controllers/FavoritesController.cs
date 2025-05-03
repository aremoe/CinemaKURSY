using Microsoft.AspNetCore.Mvc;
using Cinema.Services;
using Cinema.Entities;
using Cinema.Interfaces;

namespace Cinema.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly IFavoriteService _favService;

        public FavoritesController(IFavoriteService favService)
        {
            _favService = favService;
        }

        public IActionResult Index()
        {
            var favoriteMovies = _favService.GetAll();
            return View(favoriteMovies);
        }

        [HttpPost]
        public IActionResult Add(int id, string? returnUrl)
        {
            if (!_favService.GetIds().Contains(id))
            {
                _favService.Add(id);
            }
            return returnUrl != null ? Redirect(returnUrl) : RedirectToAction("Index", "Movies");
        }

        [HttpPost]
        public IActionResult Remove(int id, string? returnUrl)
        {
            _favService.Remove(id);
            return returnUrl != null ? Redirect(returnUrl) : RedirectToAction("Index", "Movies");
        }

        [HttpPost]
        public IActionResult ToggleFavorite(int id, string? returnUrl)
        {
            if (_favService.GetIds().Contains(id))
            {
                _favService.Remove(id);
            }
            else
            {
                _favService.Add(id);
            }
            return returnUrl != null ? Redirect(returnUrl) : RedirectToAction("Index", "Movies");
        }

        [HttpGet]
      [HttpGet]
public IActionResult GetFavoriteCount()
{
    var count = _favService.GetCount();
    return Json(new { count });
}

    }
}
