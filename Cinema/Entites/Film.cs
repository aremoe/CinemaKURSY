using System.ComponentModel.DataAnnotations;
namespace Cinema.Entities;

public class Film
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Cover { get; set; }

    public int Year { get; set; }
    //[Range(0, int.MaxValue, ErrorMessage = "Genre cannot be emphty.")]
    public string Genre { get; set; }
    [Range(0, int.MaxValue, ErrorMessage = "Duration cannot be negative.")]
    public int Duration { get; set; }
    public ICollection<Actor>? Actors { get; set; }

    //[Required(ErrorMessage = "Film team is required.")]
    //public int ActorId { get; set; }
    //public Actor? Actor { get; set; }

}