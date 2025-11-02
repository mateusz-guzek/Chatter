using Server.Shared.Models.Dtos;

namespace Server.Services.Interfaces;

public interface IChatService
{
    // chat rooms
    public Task<List<ChatRoomDto>> GetAllChatRooms();

    public Task<List<ChatRoomDto>> GetAllPublicChatRooms();
    public Task<ChatRoomDto> CreateChatRoom(string name, Guid ownerId);
    public Task<Guid> RenameChatRoom(Guid chatRoomId, string newName);
    public Task<Guid> DeleteChatRoom(Guid chatRoomId);
    public Task<ChatRoomDto> GetChatRoomById(Guid id);
    public Task<List<ChatRoomDto>> SearchChatRooms(string query);

    // users
    public Task<List<ChatRoomDto>> GetChatRoomsOfUser(Guid userId);
    public Task<bool> AddUserToChatRoom(Guid chatRoomId, Guid userId);
    public Task<bool> RemoveUserFromChatRoom(Guid chatRoomId, Guid userId);

    public Task<List<UserDto>> GetUsersInChatRoom(Guid chatRoomId);
    
    public Task<bool> IsUserInChatRoom(Guid chatRoomId, Guid userId);

    // messages
    public Task<bool> SendMessage(Guid chatRoomId, Guid userId, string message);
    public Task<List<ChatMessageDto>> GetMessagesSince(Guid chatRoomId, DateTime since, int limit = 50);
    public Task<List<ChatMessageDto>> GetRecentMessages(Guid chatRoomId, int limit = 50);
}