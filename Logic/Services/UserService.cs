using MoviesArchive.Data.Interfaces;
using MoviesArchive.Data.Models;
using MoviesArchive.Logic.Authorization;
using MoviesArchive.Logic.IServices;
using MoviesArchive.Logic.ModelsDto;

namespace MoviesArchive.Logic.Services;

internal class UserService : IUserService
{
    private readonly IUserAuthorize _userAuthorize;
    private readonly IUserRepository _userRepository;
    private readonly IIpAddressRepository _ipAddressRepository;

    public UserService(IUserAuthorize userAuthorize, IUserRepository userRepository, IIpAddressRepository ipAddressRepository)
    {
        _userRepository = userRepository;
        _userAuthorize = userAuthorize;
        _ipAddressRepository = ipAddressRepository;
    }

    public async Task<List<User>> GetUsersList()
    {
        var users = await _userRepository.GetSortedUsers();
        var userWithoutAdmin = users.Where(u => u.Role != "Admin").ToList();
        return userWithoutAdmin;
    }

    public async Task<UserDto> RegisterUser(User user, string userIp)
    {
        if (string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Email))
        {
            return new UserDto
            {
                AuthorizeSuccessful = false
            };
        }
        var nameOrEmailExists = await _userRepository.UserNameOrEmailExists(user.Name, user.Email);
        if (nameOrEmailExists)
        {
            return new UserDto
            {
                AuthorizeSuccessful = false
            };
        }

        var ipAddressDB = await _ipAddressRepository.GetIpAddress(userIp);
        user.IpAddresses = new List<IpAddress>
        {
            ipAddressDB ?? 
            new IpAddress
            {
                Value = userIp
            }
        };

        var userAdded = await _userRepository.TryAddUser(user);
        var userDto = new UserDto
        {
            AuthorizeSuccessful = userAdded
        };
        if (userAdded)
        {
            await _userAuthorize.Authorize(user);
        }
        return userDto;
    }

    public async Task<UserDto> LoginUser(string name, string password, string userIp)
    {
        var user = await _userRepository.GetUserWithIpAddress(name, password);
        if (user is null)
        {
            return new UserDto
            {
                AuthorizeSuccessful = false
            };
        }
        
        var ipAddressDB = await _ipAddressRepository.GetIpAddress(userIp);
        if (ipAddressDB is null)
        {
            var ipAddress = new IpAddress
            {
                Value = userIp
            };
            user.IpAddresses.Add(ipAddress);
        }
        else
        {
            user.IpAddresses.Add(ipAddressDB);
        }
        await _userRepository.UpdateUser(user);

        await _userAuthorize.Authorize(user);
        return new UserDto
        {
            AuthorizeSuccessful = true,
        };
    }

    public async Task<bool> UserNameExists(string name)
    {
        return await _userRepository.UserNameExists(name);
    }
}
