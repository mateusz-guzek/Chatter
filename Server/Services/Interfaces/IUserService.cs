using Server.Shared.Models.Entities;

namespace Server.Services.Interfaces;

public interface IUserService
{
    public Task<List<User>> GetAllUsers();
    public Task<User?> GetUserByName(string name);
    public Task<User?> GetUserById(Guid id);
    public Task<User> CreateUser(string name, string password);
    public Task<User> UpdateUser(User user);
    public Task<User> DeleteUser(User user);
}