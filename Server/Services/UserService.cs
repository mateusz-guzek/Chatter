using Chatter.Server.Data;
using Chatter.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Chatter.Server.Services;

public class UserService : IUserService
{
    private readonly ChatterDbContext _dbContext;
    public UserService(ChatterDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task AddUser(User user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<User> CreateUser(User user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }



    public async Task<User> DeleteUser(User user)
    {
        _dbContext.Users.Remove(user);
        return await Task.FromResult(user);
    }

    public async Task<User> UpdateUser(User user)
    {
        _dbContext.Users.Attach(user);
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }

    public async Task<User?> GetUserById(Guid id)
    {
        return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<List<User>> GetAllUsers()
    {
        return _dbContext.Users.ToListAsync();
    }

    public async Task<User?> GetUserByName(string name)
    {
        return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Name == name);
    }
    
}