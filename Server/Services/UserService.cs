using Chatter.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Shared.Models;

namespace Server.Services;

public class UserService : IUserService
{
    private readonly ChatterDbContext _dbContext;
    public UserService(ChatterDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    

    public async Task<User> CreateUser(string name, string password)
    {
        var user = new User(name, password);
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }



    public async Task<User> DeleteUser(User user)
    {
        _dbContext.Users.Remove(user);
        return await Task.FromResult(user);
    }

    public Task<User> CreateUser(User user)
    {
        throw new NotImplementedException();
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