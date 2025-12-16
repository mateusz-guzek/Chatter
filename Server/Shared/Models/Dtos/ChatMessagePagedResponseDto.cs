namespace Server.Shared.Models.Dtos;

public class ChatMessagePagedResponseDto
{
    public ChatMessagePagedResponseDto()
    {
    }

    public ChatMessagePagedResponseDto(Guid chatRoomId, List<ChatMessageDto> messages, DateTime? nextCursor, bool hasMore)
    {
        ChatRoomId = chatRoomId;
        Messages = messages;
        NextCursor = nextCursor;
        HasMore = hasMore;
    }

    public Guid ChatRoomId { get; set; }
    public List<ChatMessageDto> Messages { get; set; } = new();
    public DateTime? NextCursor { get; set; }
    public bool HasMore { get; set; }
}

public class MessagesPageDto
{
    public List<ChatMessageDto> Messages { get; set; } = new();
    public DateTime? NextCursor { get; set; }
    public bool HasMore { get; set; }
}
