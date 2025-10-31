using MoviesArchive.Data.Models;

namespace MoviesArchive.Data.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUser(string name, string password);
    Task<List<User>> GetSortedUsers();
    Task<int> AddUser(User user);
    Task<bool> UserNameExists(string name);
    Task<bool> UserNameOrEmailExists(string name, string email);
}
