using MoviesArchive.Data.Models;

namespace MoviesArchive.Logic.Authorization;

public interface IUserAuthorize
{
    Task Authorize(User user);
}
