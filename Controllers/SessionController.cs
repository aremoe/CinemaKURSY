using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cinema.Data;
using Cinema.Entities;
using Cinema.Interfaces;
using System.Security.Claims;

namespace Cinema.Controllers
{
    public class SessionController : Controller
    {
        private readonly MovieDbContext _context;
        private readonly ISeatService _seatService;

        public SessionController(MovieDbContext context, ISeatService seatService)
        {
            _context = context;
            _seatService = seatService;
        }

        // GET: Session/Details/{id}
        public IActionResult Details(int id)
        {
            var session = _seatService.GetSessionWithSeats(id);
            if (session == null)
            {
                return NotFound();
            }

            ViewBag.CurrentCustomerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(session);
        }

        // POST: Session/Reserve
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reserve(int sessionId, int seatNumber)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var success = _seatService.ReserveSeat(sessionId, seatNumber, userId);
            if (!success)
            {
                TempData["Error"] = "Seat is already reserved or does not exist.";
                return RedirectToAction("Details", new { id = sessionId });
            }

            TempData["Success"] = "Seat reserved successfully!";
            return RedirectToAction("Details", new { id = sessionId });
        }

        // GET: Session/ReservedSeats
        public IActionResult ReservedSeats()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var reservedSeats = _seatService.GetReservedSeatsByUser(userId);
            return View(reservedSeats);
        }

        // GET: Session/Index
        public IActionResult Index()
        {
            var sessions = _context.Sessions
                .Include(s => s.Movie)
                .ToList();

            return View(sessions);
        }

        // GET: Session/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Session/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Session session)
        {
            if (ModelState.IsValid)
            {
                _context.Sessions.Add(session);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(session);
        }

        // GET: Session/Edit/{id}
        public IActionResult Edit(int id)
        {
            var session = _context.Sessions.Find(id);
            if (session == null)
            {
                return NotFound();
            }
            return View(session);
        }

        // POST: Session/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Session session)
        {
            if (id != session.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _context.Update(session);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(session);
        }

        // GET: Session/Delete/{id}
        public IActionResult Delete(int id)
        {
            var session = _context.Sessions.Find(id);
            if (session == null)
            {
                return NotFound();
            }
            return View(session);
        }

        // POST: Session/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var session = _context.Sessions.Find(id);
            if (session != null)
            {
                _context.Sessions.Remove(session);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
