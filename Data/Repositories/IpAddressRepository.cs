using Microsoft.EntityFrameworkCore;
using MoviesArchive.Data.Context;
using MoviesArchive.Data.Interfaces;
using MoviesArchive.Data.Models;

namespace MoviesArchive.Data.Repositories;

internal class IpAddressRepository : IIpAddressRepository
{
    private readonly ApplicationContext _db;

    public IpAddressRepository(ApplicationContext database)
    {
        _db = database;
    }

    public async Task<IpAddress?> GetIpAddress(string ipValue)
    {
        var ipAddress = await _db.IpAddresses.FirstOrDefaultAsync(ip => ip.Value == ipValue);
        return ipAddress;
    }
}
