namespace Cinema.Entities;

public class Actor
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int FilmId { get; set; }
    public Film Film { get; set; }
}