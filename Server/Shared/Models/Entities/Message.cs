namespace Server.Shared.Models.Entities;

public class Message
{
    public Message()
    {
    }

    public Message(User sender, string text, ChatRoom chatRoom)
    {
        Sender = sender;
        Text = text;
        ChatRoom = chatRoom;
        Date = DateTime.UtcNow;
    }

    public Guid Id { get; set; }
    public User Sender { get; set; }
    public string Text { get; set; }
    public DateTime Date { get; set; }
    public ChatRoom ChatRoom { get; set; }
}