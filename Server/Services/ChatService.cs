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
        var chatRoom = new ChatRoom(name, owner);
        _db.ChatRooms.Add(chatRoom);
        await _db.SaveChangesAsync();
        return chatRoom;
    }
    public async Task<ChatRoom> UpdateChatRoom(ChatRoom chatRoom)
    {
        _db.ChatRooms.Update(chatRoom);
        await _db.SaveChangesAsync();
        return chatRoom;
    }
    public async Task<ChatRoom> DeleteChatRoom(ChatRoom chatRoom)
    {
        _db.ChatRooms.Remove(chatRoom);
        await _db.SaveChangesAsync();
        return chatRoom;
    }
    public async Task<ChatRoom> GetChatRoomById(Guid id)
    {
        return await _db.ChatRooms.FindAsync(id) ?? throw new InvalidOperationException();
    }
    public async Task<IEnumerable<ChatRoom>> SearchChatRooms(string query)
    {
        return await _db.ChatRooms.Where(x => x.Name.Contains(query)).ToListAsync();
    }
    public async Task<IEnumerable<ChatRoom>> GetUsersChatRooms(Guid userId)
    {
        return await _db.Users.Where(x => x.Id == userId).SelectMany(x => x.ChatRooms).ToListAsync();
    }
    public async Task<bool> AddUserToChatRoom(Guid chatRoomId, Guid userId)
    {
        var chatRoom = await GetChatRoomById(chatRoomId);
        var user = await _db.Users.FindAsync(userId);
        if (user == null) return false;
        chatRoom.Users.Add(user);
        await _db.SaveChangesAsync();
        return true;
    }
    public async Task<bool> RemoveUserFromChatRoom(Guid chatRoomId, Guid userId)
    {
        var chatRoom = await GetChatRoomById(chatRoomId);
        var user = await _db.Users.FindAsync(userId);
        if (user == null) return false;
        chatRoom.Users.Remove(user);
        await _db.SaveChangesAsync();
        return true;
    }
    public async Task<IEnumerable<User>> GetUsersInChatRoom(Guid chatRoomId)
    {
        var chatRoom = await GetChatRoomById(chatRoomId);
        return chatRoom.Users;
    }
    public async Task<bool> SendMessage(Guid chatRoomId, Guid userId, string message)
    {
        var chatRoom = await GetChatRoomById(chatRoomId);
        var user = await _db.Users.FindAsync(userId);
        if (user == null) return false;
        var chatMessage = new Message(user, message, chatRoom);
        chatRoom.Messages.Add(chatMessage);
        await _db.SaveChangesAsync();
        return true;
    }
    public async Task<IEnumerable<Message>> GetMessagesSince(Guid chatRoomId, DateTime since)
    {
        var chatRoom = await GetChatRoomById(chatRoomId);
        return chatRoom.Messages.Where(x => x.Date > since);
    }
}