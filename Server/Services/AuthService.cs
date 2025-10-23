using System.Security.Cryptography;
using Chatter.Server.Data;
using Chatter.Server.Services.Interfaces;
using Chatter.Server.Utility;
using Microsoft.Extensions.Caching.Memory;
using Server.Models;

namespace Chatter.Server.Services;

public class AuthService : IAuthService
{
    private readonly ChatterDbContext _dbContext;
    private readonly IUserService _userService;
    private readonly IMemoryCache _memoryCache;
    
    public AuthService(ChatterDbContext dbContext, IUserService userService, IMemoryCache memoryCache)
    {
        _dbContext = dbContext;
        _userService = userService;
        _memoryCache = memoryCache;
    }
    public async Task<bool> Authenticate(string username, string password)
    {
        var user = await _userService.GetUserByName(username);
        if (user == null) return false;

        return Passwords.VerifyPassword(password, user.Password);

        // if (!Passwords.VerifyPassword(password, user.Password)) throw new InvalidOperationException("Invalid username or password");
        // var token = GenerateToken();
        // _memoryCache.Set(token, user.Id, TimeSpan.FromHours(6));
        // return token;
    }
    public async Task<bool> Register(string username, string password)
    {
        var user = await _userService.GetUserByName(username);
        if (user != null) return false;
        var hashedPassword = Passwords.HashPassword(password);
        var newUser = new User
        {
            Name = username,
            Password = hashedPassword
        };
        await _userService.CreateUser(newUser);
        return true;
    }
    public async Task<bool> ChangePassword(string username, string oldPassword, string newPassword)
    {
        var user = await _userService.GetUserByName(username);
        if (user == null) return false;
        if (!Passwords.VerifyPassword(oldPassword, user.Password)) return false;
        var hashedPassword = Passwords.HashPassword(newPassword);
        user.Password = hashedPassword;
        await _userService.UpdateUser(user);
        return true;
    }

    public Task<string> GetId(string username, string password)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Logout(string token)
    {
        if (!_memoryCache.TryGetValue(token, out Guid userId)) return false;
        _memoryCache.Remove(token);
        return true;
    }

    public async Task<string> GetId(string username)
    {
        var user = await _userService.GetUserByName(username);
        if (user == null) throw new InvalidOperationException("Invalid username or password");
        return user.Id.ToString();
    }

    private string GenerateToken()
    {
        var tokenBytes = new byte[32];
        RandomNumberGenerator.Fill(tokenBytes);
        return Convert.ToBase64String(tokenBytes);
    }
}