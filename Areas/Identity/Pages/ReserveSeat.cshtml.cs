using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Cinema.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Cinema.Pages
{
    public class ReserveSeatModel : PageModel
    {
        private readonly ISeatService _seatService;

        public ReserveSeatModel(ISeatService seatService)
        {
            _seatService = seatService;
        }

        [BindProperty]
        [Required(ErrorMessage = "Seat number is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seat number must be a positive integer.")]
        public int SeatNumber { get; set; }

        [BindProperty(SupportsGet = true)]
        public int SessionId { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Unauthorized();
            }

            var success = _seatService.ReserveSeat(SessionId, SeatNumber, userId);
            if (!success)
            {
                ModelState.AddModelError(string.Empty, "The seat is already reserved.");
                return Page();
            }

            TempData["SuccessMessage"] = "Seat reserved successfully!";
            return RedirectToPage("/Session/Details", new { id = SessionId });
        }
    }
}
