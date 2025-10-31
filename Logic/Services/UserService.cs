﻿using MoviesArchive.Data.Enums;
using MoviesArchive.Data.Interfaces;
using MoviesArchive.Data.Models;
using MoviesArchive.Logic.Authorization;
using MoviesArchive.Logic.IServices;
using Serilog;

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

    public async Task<ResultStatus> RegisterUser(User user, string userIp)
    {
        if (string.IsNullOrWhiteSpace(user.Name) || string.IsNullOrWhiteSpace(user.Email))
        {
            return ResultStatus.Failed;
        }
        var nameOrEmailExists = await _userRepository.UserNameOrEmailExists(user.Name, user.Email);
        if (nameOrEmailExists)
        {
            return ResultStatus.Failed;
        }

        var ipAddressDB = await _ipAddressRepository.GetIpAddressWithUsers(userIp);
        user.IpAddresses = new List<IpAddress>
        {
            ipAddressDB ?? 
            new IpAddress
            {
                Value = userIp
            }
        };

        var result = await _userRepository.AddUser(user);
        if (result == 0)
        {
            Log.Warning($"{nameof(_userRepository.AddUser)} has written 0 state entries");
            return ResultStatus.Failed;
        }
        await _userAuthorize.Authorize(user);
        return ResultStatus.Success;
    }

    public async Task<ResultStatus> LoginUser(string name, string password, string userIp)
    {
        var user = await _userRepository.GetUser(name, password);
        if (user is null)
        {
            return ResultStatus.NotFound;
        }
        
        var ipAddressDB = await _ipAddressRepository.GetIpAddressWithUsers(userIp);
        if (ipAddressDB is null)
        {
            var ipAddress = new IpAddress
            {
                Value = userIp,
                Users = new List<User>
                {
                    user,
                }
            };
            var result = await _ipAddressRepository.AddIpAddress(ipAddress);
            if (result == 0)
            {
                Log.Warning($"{nameof(_ipAddressRepository.AddIpAddress)} has written 0 state entries");
            }
        }
        else if (!ipAddressDB.Users.Contains(user))
        {
            ipAddressDB.Users.Add(user);
            var result = await _ipAddressRepository.UpdateIpAddress(ipAddressDB);
            if (result == 0)
            {
                Log.Warning($"{nameof(_ipAddressRepository.UpdateIpAddress)} has written 0 state entries");
            }
        }
        await _userAuthorize.Authorize(user);
        return ResultStatus.Success;
    }

    public async Task<bool> UserNameExists(string name)
    {
        return await _userRepository.UserNameExists(name);
    }
}
