using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Services.Interfaces;
using Server.Shared.Models;
using Server.Shared.Models.Dtos;
using Server.Shared.Models.Entities;

namespace Server.Services;

public class ChatService : IChatService
{
    private readonly ChatterDbContext _db;

    public ChatService(ChatterDbContext dbContext)
    {
        _db = dbContext;
    }


    public async Task<List<ChatRoomDto>> GetAllChatRooms()
    {
        return await _db.ChatRooms
            .Include(x => x.Owner)
            .Include(x => x.Users)
            .Select(x => new ChatRoomDto(x))
            .ToListAsync();
    }

    public Task<List<ChatRoomDto>> GetAllPublicChatRooms()
    {
        return _db.ChatRooms
            .Include(x => x.Owner)
            .Include(x => x.Users)
            .Where(x => !x.isPrivate)
            .Select(x => new ChatRoomDto(x))
            .ToListAsync();
    }

    public async Task<ChatRoomDto> CreateChatRoom(string name, Guid ownerId)
    {
        var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == ownerId);
        var room = new ChatRoom(name, user);
        room.Users.Add(user);
        _db.ChatRooms.Add(room);
        await _db.SaveChangesAsync();
        return new ChatRoomDto(room);
    }

    public async Task<Guid> RenameChatRoom(Guid chatRoomId, string newName)
    {
        var room = await _db.ChatRooms.FirstOrDefaultAsync(x => x.Id == chatRoomId);
        room.Name = newName;
        await _db.SaveChangesAsync();
        return room.Id;
    }

    public async Task<Guid> DeleteChatRoom(Guid chatRoomId)
    {
        var room = await _db.ChatRooms
            .FirstOrDefaultAsync(x => x.Id == chatRoomId);
        _db.ChatRooms.Remove(room);
        await _db.SaveChangesAsync();
        return room.Id;
    }

    public async Task<ChatRoomDto> GetChatRoomById(Guid id)
    {
        var room = await _db.ChatRooms
            .Include(x => x.Owner)
            .Include(x => x.Users)
            .FirstOrDefaultAsync(x => x.Id == id);
        return new ChatRoomDto(room);
    }

    public async Task<List<ChatRoomDto>> SearchChatRooms(string query)
    {
        var rooms = await _db.ChatRooms
            .Include(x => x.Owner)
            .Include(x => x.Users)
            .Where(x => x.Name.Contains(query))
            .ToListAsync();
        return rooms.Select(x => new ChatRoomDto(x)).ToList();
    }

    public async Task<List<ChatRoomDto>> GetChatRoomsOfUser(Guid userId)
    {
        return await _db.ChatRooms
            .Include(x => x.Owner)
            .Include(x => x.Users)
            .Where(x => x.Users.Any(y => y.Id == userId))
            .Select(x => new ChatRoomDto(x))
            .ToListAsync();
    }

    public async Task<bool> AddUserToChatRoom(Guid chatRoomId, Guid userId)
    {
        var room = await _db.ChatRooms.FirstOrDefaultAsync(x => x.Id == chatRoomId);
        var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);
        room.Users.Add(user);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveUserFromChatRoom(Guid chatRoomId, Guid userId)
    {
        var room = await _db.ChatRooms.FirstOrDefaultAsync(x => x.Id == chatRoomId);
        var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);
        room.Users.Remove(user);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<UserDto>> GetUsersInChatRoom(Guid chatRoomId)
    {
        var room = await _db.ChatRooms.FirstOrDefaultAsync(x => x.Id == chatRoomId);
        return room.Users.Select(x => new UserDto(x)).ToList();
    }

    public async Task<bool> IsUserInChatRoom(Guid chatRoomId, Guid userId)
    {
        return await _db.Users
            .Where(u => u.Id == userId)
            .AnyAsync(u => u.ChatRooms.Any(cr => cr.Id == chatRoomId));
    }

    public async Task<bool> SendMessage(Guid chatRoomId, Guid userId, string message)
    {
        var room = await _db.ChatRooms.FirstOrDefaultAsync(x => x.Id == chatRoomId);
        var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);
        var messag = new Message(user, message, room);
        room.Messages.Add(messag);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<ChatMessageDto>> GetMessagesSince(Guid chatRoomId, DateTime since, int limit = 50)
    {
        var messages = await _db.Messages
            .Where(x => x.ChatRoom.Id == chatRoomId && x.Date > since)
            .OrderByDescending(x => x.Date)
            .Take(limit)
            .Select(x => new ChatMessageDto
            {
                Id = x.Id,
                Text = x.Text,
                Timestamp = x.Date,
                Sender = new UserDto
                {
                    Id = x.Sender.Id,
                    Name = x.Sender.Name
                }
            })
            .ToListAsync();

        return messages;
    }

    public async Task<List<ChatMessageDto>> GetRecentMessages(Guid chatRoomId, int limit = 50)
    {
        var messages = await _db.Messages
            .Where(x => x.ChatRoom.Id == chatRoomId)
            .OrderByDescending(x => x.Date)
            .Take(limit)
            .Select(x => new ChatMessageDto
            {
                Id = x.Id,
                Text = x.Text,
                Timestamp = x.Date,
                Sender = new UserDto
                {
                    Id = x.Sender.Id,
                    Name = x.Sender.Name
                }
            })
            .ToListAsync();

        return messages;
    }
}