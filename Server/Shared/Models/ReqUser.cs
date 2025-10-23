namespace Server.Shared.Models;

public class ReqUser
{
    public string Id { get; set; }
    public string Name { get; set; }
    public ReqUser(string id, string name)
    {
        Id = id;
        Name = name;
    }
}