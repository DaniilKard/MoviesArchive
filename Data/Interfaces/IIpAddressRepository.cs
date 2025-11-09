using MoviesArchive.Data.Models;

namespace MoviesArchive.Data.Interfaces;

public interface IIpAddressRepository
{
    Task<IpAddress?> GetIpAddress(string ipValue);
    Task<IpAddress?> GetIpAddressWithUsers(string ipValue);
    Task<int> AddIpAddress(IpAddress ipAddress);
    Task<int> UpdateIpAddress(IpAddress ipAddress);
}
