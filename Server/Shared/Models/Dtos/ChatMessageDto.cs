using Server.Shared.Models.Entities;

namespace Server.Shared.Models.Dtos;

public class ChatMessageDto
{
    public ChatMessageDto()
    {
    }

    public ChatMessageDto(Message message)
    {
        Id = message.Id;
        Text = message.Text;
        Sender = new UserDto(message.Sender);
        Timestamp = message.Date;
    }

    public Guid Id { get; set; }
    public string Text { get; set; }
    public UserDto Sender { get; set; }
    public DateTime Timestamp { get; set; }
}