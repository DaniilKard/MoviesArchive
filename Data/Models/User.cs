using Microsoft.EntityFrameworkCore;

namespace MoviesArchive.Data.Models;

[Index(nameof(Name), IsUnique = true)]
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateOnly RegistrationDate { get; set; }
    public string Role { get; set; }
    public List<Movie>? Movies { get; set; }
    public List<IpAddress>? IpAddresses { get; set; }
}
