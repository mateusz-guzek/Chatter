namespace Server.Shared.Models;

public class User
{
    public User() {}
    public User(string name, string password)
    {
        Name = name;
        Password = password;
    }
    public Guid Id { get; init; }
    public string Name { get; set; }
    public string Password { get; set; }

    public List<ChatRoom> ChatRooms { get; set; } = [];
    
    public List<Message> Messages { get; set; } = [];
}