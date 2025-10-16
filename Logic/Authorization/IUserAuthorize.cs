using MoviesArchive.Data.Models;
using System.Security.Claims;

namespace MoviesArchive.Logic.Authorization;

public interface IUserAuthorize
{
    Task Authorize(User user);
}
