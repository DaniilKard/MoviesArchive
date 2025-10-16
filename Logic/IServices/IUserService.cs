using MoviesArchive.Data.Models;
using MoviesArchive.Logic.ModelsDto;

namespace MoviesArchive.Logic.IServices;

public interface IUserService
{
    Task<List<User>> GetUsersList();
    Task<UserDto> LoginUser(string name, string password, string userIp);
    Task<UserDto> RegisterUser(User user, string userIp);
    Task<bool> UserNameExists(string name);
}
