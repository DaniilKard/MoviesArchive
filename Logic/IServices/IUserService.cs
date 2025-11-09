using MoviesArchive.Data.Enums;
using MoviesArchive.Data.Models;

namespace MoviesArchive.Logic.IServices;

public interface IUserService
{
    Task<List<User>> GetUsersList();
    Task<ResultStatus> LoginUser(string name, string password, string userIp);
    Task<ResultStatus> RegisterUser(string name, string email, string password, string userIp);
    Task<bool> UserNameExists(string name);
}
