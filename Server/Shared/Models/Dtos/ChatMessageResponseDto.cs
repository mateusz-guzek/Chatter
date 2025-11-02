namespace Server.Shared.Models.Dtos;

public class ChatMessageResponseDto
{
    public ChatMessageResponseDto()
    {
    }

    public ChatMessageResponseDto(Guid chatRoomId, List<ChatMessageDto> messages)
    {
        ChatRoomId = chatRoomId;
        Messages = messages;
    }

    public Guid ChatRoomId { get; set; }
    public List<ChatMessageDto> Messages { get; set; }
}