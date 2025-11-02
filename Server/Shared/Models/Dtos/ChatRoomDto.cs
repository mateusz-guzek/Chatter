using Server.Shared.Models.Entities;

namespace Server.Shared.Models.Dtos;

public class ChatRoomDto
{
    public ChatRoomDto()
    {
    }

    public ChatRoomDto(ChatRoom chatRoom)
    {
        Id = chatRoom.Id;
        Name = chatRoom.Name;
        Owner = new UserDto(chatRoom.Owner);
        Users = chatRoom.Users.Select(x => new UserDto(x)).ToList();
        isPrivate = chatRoom.isPrivate;
    }

    public Guid Id { get; set; }

    public bool isPrivate { get; set; }
    public string Name { get; set; }
    public UserDto Owner { get; set; }
    public List<UserDto> Users { get; set; }
}