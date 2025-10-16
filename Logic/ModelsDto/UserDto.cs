using System.Security.Claims;

namespace MoviesArchive.Logic.ModelsDto;

public class UserDto
{
    public bool AuthorizeSuccessful { get; set; }
    public ClaimsIdentity? ClaimsIdentity { get; set; }
}
