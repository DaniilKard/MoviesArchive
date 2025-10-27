using Microsoft.EntityFrameworkCore;
using MoviesArchive.Data.Context;
using MoviesArchive.Data.Interfaces;
using MoviesArchive.Data.Models;
using Serilog;

namespace MoviesArchive.Data.Repositories;

internal class IpAddressRepository : IIpAddressRepository
{
    private readonly ApplicationContext _db;

    public IpAddressRepository(ApplicationContext database)
    {
        _db = database;
    }

    public async Task<IpAddress?> GetIpAddressWithUsers(string ipValue)
    {
        var ipAddress = await _db.IpAddresses.Include(ip => ip.Users).FirstOrDefaultAsync(ip => ip.Value == ipValue);
        return ipAddress;
    }

    public async Task<int> AddIpAddress(IpAddress ipAddress)
    {
        _db.IpAddresses.Add(ipAddress);
        var result = await _db.SaveChangesAsync();
        if (result == 0)
        {
            Log.Warning("AddIpAddress has written 0 state entries");
        }
        return result;
    }

    public async Task<int> UpdateIpAddress(IpAddress ipAddress)
    {
        _db.IpAddresses.Update(ipAddress);
        var result = await _db.SaveChangesAsync();
        if (result == 0)
        {
            Log.Warning("UpdateIpAddressUsers has written 0 state entries");
        }
        return result;
    }
}
