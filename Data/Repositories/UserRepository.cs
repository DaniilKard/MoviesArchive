using Microsoft.EntityFrameworkCore;
using MoviesArchive.Data.Context;
using MoviesArchive.Data.Interfaces;
using MoviesArchive.Data.Models;

namespace MoviesArchive.Data.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly ApplicationContext _db;

    public UserRepository(ApplicationContext database)
    {
        _db = database;
    }

    public async Task<User?> GetUser(string name)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Name == name);
        return user;
    }

    public async Task<List<User>> GetSortedUsers()
    {
        var users = await _db.Users.OrderBy(u => u.Id).ToListAsync();
        return users;
    }

    public async Task<int> AddUser(User user)
    {
        _db.Users.Add(user);
        var result = await _db.SaveChangesAsync();
        return result;
    }

    public async Task<bool> UserNameExists(string name)
    {
        var nameExists = await _db.Users.AsNoTracking().AnyAsync(u => u.Name.ToLower() == name.ToLower());
        return nameExists;
    }

    public async Task<bool> UserNameOrEmailExists(string name, string email)
    {
        var nameOrEmailExists = await _db.Users.AsNoTracking().AnyAsync(u => u.Email.ToLower() == email.ToLower() || u.Name.ToLower() == name.ToLower());
        return nameOrEmailExists;
    }
}
