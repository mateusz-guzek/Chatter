using Server.Shared.Models;

namespace Server.Services.Interfaces;

public interface IChatService
{
    public Task<List<ChatRoom>> GetAllChatRooms();
    
    public Task<ChatRoom> CreateChatRoom(string name, User owner);
    public Task<ChatRoom> UpdateChatRoom(ChatRoom chatRoom);
    public Task<ChatRoom> DeleteChatRoom(ChatRoom chatRoom);
    public Task<ChatRoom> GetChatRoomById(Guid id);
    public Task<bool> AddUserToChatRoom(Guid chatRoomId, Guid userId);
    public Task<bool> RemoveUserFromChatRoom(Guid chatRoomId, Guid userId);
    public Task<IEnumerable<User>> GetUsersInChatRoom(Guid chatRoomId);
    public Task<bool> SendMessage(Guid chatRoomId, Guid userId, string message);
    public Task<IEnumerable<ChatRoom>> SearchChatRooms(string query);
    public Task<IEnumerable<ChatRoom>> GetUsersChatRooms(Guid userId);
    
    
    
}