using Server.Shared.Models.Entities;

namespace Server.Shared.Models.Dtos;

public class UserDto
{
    public UserDto()
    {
    }

    public UserDto(User user)
    {
        Id = user.Id;
        Name = user.Name;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
}