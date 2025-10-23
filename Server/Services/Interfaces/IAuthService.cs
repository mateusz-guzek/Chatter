namespace Chatter.Server.Services.Interfaces;

public interface IAuthService
{
    public Task<bool> Authenticate(string username, string password);
    public Task<bool> Register(string username, string password);
    public Task<bool> ChangePassword(string username, string oldPassword, string newPassword);
    
    public Task<string> GetId(string username);
}