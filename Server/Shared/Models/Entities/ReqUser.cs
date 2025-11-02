namespace Server.Shared.Models.Entities;

public class ReqUser
{
    public ReqUser(string id, string name)
    {
        Id = id;
        Name = name;
    }

    public string Id { get; set; }
    public string Name { get; set; }
}