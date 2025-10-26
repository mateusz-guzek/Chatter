namespace Server.Shared.Models;

public class ChatRoom
{
    
    public ChatRoom() {}
    public ChatRoom(string name, User owner)
    {
        Name = name;
        Owner = owner;
    }
    
    public Guid Id { get; set; }
    
    public User Owner { get; set; }
    public string Name { get; set; }
    public List<User> Users { get; set; } = [];
    public List<Message> Messages { get; set; } = [];
    
}