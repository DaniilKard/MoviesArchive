using Microsoft.EntityFrameworkCore;
using MoviesArchive.Data.Context;
using MoviesArchive.Data.Interfaces;
using MoviesArchive.Data.Models;
using Serilog;

namespace MoviesArchive.Data.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly ApplicationContext _db;

    public UserRepository(ApplicationContext database)
    {
        _db = database;
    }

    public async Task<User?> GetUserWithIpAddress(string name, string password)
    {
        var user = await _db.Users.Include(u => u.IpAddresses).FirstOrDefaultAsync(u => u.Name == name && u.Password == password);
        return user;
    }

    public async Task<List<User>> GetSortedUsers()
    {
        var users = await _db.Users.OrderBy(u => u.Id).ToListAsync();
        return users;
    }

    public async Task<bool> TryAddUser(User user)
    {
        _db.Users.Add(user);
        var result = await _db.SaveChangesAsync();
        if (result == 0)
        {
            Log.Warning("TryAddUserToNewIp has written 0 state entries");
            return false;
        }
        return true;
    }

    public async Task UpdateUser(User user)
    {
        _db.Users.Update(user);
        var result = await _db.SaveChangesAsync();
        if (result == 0)
        {
            Log.Warning("UpdateUser has written 0 state entries");
        }
    }

    public async Task<bool> UserNameExists(string name)
    {
        var nameExists = await _db.Users.AsNoTracking().AnyAsync(u => u.Name.ToLower() == name.ToLower());
        return nameExists;
    }

    public async Task<bool> UserNameOrEmailExists(string name, string email)
    {
        var nameOrEmailExists = await _db.Users.AsNoTracking().AnyAsync(u => 
        u.Email.ToLower() == email.ToLower() || u.Name.ToLower() == name.ToLower());
        return nameOrEmailExists;
    }
}
