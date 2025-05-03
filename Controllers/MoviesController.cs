using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Cinema.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cinema.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MovieDbContext _context;
        private readonly IEmailSender _emailSender;

        public MoviesController(MovieDbContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Movies.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id, string? returnUrl = null)
        {
            if (id == null) return NotFound();

            var movie = await _context.Movies.FindAsync(id);

            if (movie == null) return NotFound();

            ViewBag.ReturnUrl = returnUrl;
            return View(movie);
        }

        public IActionResult Create()
        {
            ViewBag.Genres = new SelectList(new[] { "Action", "Drama", "Comedy", "Horror", "Sci-Fi" });
            ViewBag.Countries = new SelectList(new[] { "USA", "UK", "Canada", "Australia", "India" });
            ViewBag.Directors = new SelectList(_context.Directors, "Id", "Name");
            ViewBag.Actors = new MultiSelectList(_context.Actors, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Movie movie, int[] selectedActors)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Genres = new SelectList(new[] { "Action", "Drama", "Comedy", "Horror", "Sci-Fi" });
                ViewBag.Countries = new SelectList(new[] { "USA", "UK", "Canada", "Australia", "India" });
                ViewBag.Directors = new SelectList(_context.Directors, "Id", "Name", movie.DirectorId);
                ViewBag.Actors = new MultiSelectList(_context.Actors, "Id", "Name", selectedActors);
                return View(movie);
            }

            movie.Actors = new List<Actor>();
            foreach (var actorId in selectedActors)
            {
                var actor = await _context.Actors.FindAsync(actorId);
                if (actor != null)
                {
                    movie.Actors.Add(actor);
                }
            }

            _context.Add(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var movie = await _context.Movies
                .Include(m => m.Actors)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null) return NotFound();

            ViewBag.Genres = new SelectList(new[] { "Action", "Drama", "Comedy", "Horror", "Sci-Fi" }, movie.Genre);
            ViewBag.Countries = new SelectList(new[] { "USA", "UK", "Canada", "Australia", "India" }, movie.Country);
            ViewBag.Directors = new SelectList(_context.Directors, "Id", "Name", movie.DirectorId);
            ViewBag.Actors = new MultiSelectList(_context.Actors, "Id", "Name", movie.Actors.Select(a => a.Id));
            return View(movie);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Movie movie, int[] selectedActors)
        {
            if (id != movie.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Genres = new SelectList(new[] { "Action", "Drama", "Comedy", "Horror", "Sci-Fi" }, movie.Genre);
                ViewBag.Countries = new SelectList(new[] { "USA", "UK", "Canada", "Australia", "India" }, movie.Country);
                ViewBag.Directors = new SelectList(_context.Directors, "Id", "Name", movie.DirectorId);
                ViewBag.Actors = new MultiSelectList(_context.Actors, "Id", "Name", selectedActors);
                return View(movie);
            }

            var movieToUpdate = await _context.Movies
                .Include(m => m.Actors)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movieToUpdate == null) return NotFound();

            movieToUpdate.Title = movie.Title;
            movieToUpdate.Year = movie.Year;
            movieToUpdate.Description = movie.Description;
            movieToUpdate.Genre = movie.Genre;
            movieToUpdate.Duration = movie.Duration;
            movieToUpdate.CoverImage = movie.CoverImage;
            movieToUpdate.Country = movie.Country;
            movieToUpdate.TrailerUrl = movie.TrailerUrl;
            movieToUpdate.DirectorId = movie.DirectorId;

            movieToUpdate.Actors.Clear();
            foreach (var actorId in selectedActors)
            {
                var actor = await _context.Actors.FindAsync(actorId);
                if (actor != null)
                {
                    movieToUpdate.Actors.Add(actor);
                }
            }

            _context.Update(movieToUpdate);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var movie = await _context.Movies.FindAsync(id);

            if (movie == null) return NotFound();

            await _emailSender.SendEmailAsync("tymo.dimav@gmail.com", "Deleting team",
     $"<h1>Team on deleting...</h1><p>{movie.Title}</p>");


            return View(movie);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null) return NoContent();

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}