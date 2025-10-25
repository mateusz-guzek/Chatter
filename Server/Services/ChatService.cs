using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Services.Interfaces;
using Server.Shared.Models;

namespace Server.Services;

public class ChatService : IChatService
{
    
    private readonly ChatterDbContext _db;
    public ChatService(ChatterDbContext dbContext)
    {
        _db = dbContext;
        
    }
    
    public async Task<List<ChatRoom>> GetAllChatRooms()
    {
        return await _db.ChatRooms.ToListAsync();
    }
    public async Task<ChatRoom> CreateChatRoom(string name, User owner)
    {
        
        throw new NotImplementedException();
    }
    public async Task<ChatRoom> UpdateChatRoom(ChatRoom chatRoom)
    {
        throw new NotImplementedException();
    }
    public async Task<ChatRoom> DeleteChatRoom(ChatRoom chatRoom)
    {
        throw new NotImplementedException();
    }
    public async Task<ChatRoom> GetChatRoomById(Guid id)
    {
        throw new NotImplementedException();
    }
    public async Task<IEnumerable<ChatRoom>> SearchChatRooms(string query)
    {
        throw new NotImplementedException();
    }
    public async Task<IEnumerable<ChatRoom>> GetUsersChatRooms(Guid userId)
    {
        throw new NotImplementedException();
    }
    public async Task<bool> AddUserToChatRoom(Guid chatRoomId, Guid userId)
    {
        throw new NotImplementedException();
    }
    public async Task<bool> RemoveUserFromChatRoom(Guid chatRoomId, Guid userId)
    {
        throw new NotImplementedException();
    }
    public async Task<IEnumerable<User>> GetUsersInChatRoom(Guid chatRoomId)
    {
        throw new NotImplementedException();
    }
    public async Task<bool> SendMessage(Guid chatRoomId, Guid userId, string message)
    {
        throw new NotImplementedException();
    }
    public async Task<IEnumerable<Message>> GetMessagesSince(Guid chatRoomId, DateTime since)
    {
        throw new NotImplementedException();
    }
}