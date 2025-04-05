using Microsoft.AspNetCore.Identity;

namespace Cinema.Entities;

public class User : IdentityUser
{
    public string Address { get; set; }
    public DateTime Birthdate { get; set; }

}