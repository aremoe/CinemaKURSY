using Cinema.Entities;

namespace Cinema.Models
{
    public class AvailableSeatsModel
    {
        public int SessionId { get; set; }
        public IEnumerable<Seat> AvailableSeats { get; set; } = new List<Seat>();
        public int AvailableSeatsCount { get; set; }
    }
}