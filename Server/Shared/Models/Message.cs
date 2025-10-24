namespace Server.Shared.Models;

public class Message
{
    public Guid Id { get; set; }
    public User Sender { get; set; }
    public string Text { get; set; }
    public DateTime Date { get; set; }
    public ChatRoom ChatRoom { get; set; }
    
}