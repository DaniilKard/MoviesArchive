using MoviesArchive.Data.Models;

namespace MoviesArchive.Data.Interfaces;

public interface IIpAddressRepository
{
    Task<IpAddress?> GetIpAddress(string ipValue);
}
