using MoviesArchive.Data.Models;

namespace MoviesArchive.Data.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserWithIpAddress(string name, string password);
    Task<List<User>> GetSortedUsers();
    Task<bool> TryAddUser(User user);
    Task<bool> UserNameExists(string name);
    Task<bool> UserNameOrEmailExists(string name, string email);
    Task UpdateUser(User user);
}
