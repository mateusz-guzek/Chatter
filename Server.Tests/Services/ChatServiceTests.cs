using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Server.Data;
using Server.Services;
using Server.Shared.Models.Entities;
using Xunit;

namespace Server.Tests.Services;

public class ChatServiceTests : IDisposable
{
    private readonly ChatterDbContext _context;
    private readonly ChatService _chatService;
    
    public ChatServiceTests()
    {
        var options = new DbContextOptionsBuilder<ChatterDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        _context = new ChatterDbContext(options);
        _chatService = new ChatService(_context);
    }
    
    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
    
    [Fact]
    public async Task CreateChatRoom_ShouldCreateRoom_WithValidData()
    {
        // Arrange
        var user = new User { Password = "",  Id = Guid.NewGuid(), Name = "TestUser" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        
        // Act
        var result = await _chatService.CreateChatRoom("Test Room", user.Id);
        
        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Test Room");
        result.Owner.Id.Should().Be(user.Id);
        result.Users.Should().Contain(u => u.Id == user.Id);
    }
    
    [Fact]
    public async Task GetAllPublicChatRooms_ShouldReturnOnlyPublicRooms()
    {
        // Arrange
        var user = new User { Password = "",  Id = Guid.NewGuid(), Name = "Owner" };
        _context.Users.Add(user);
        
        var publicRoom = new ChatRoom("Public Room", user) { isPrivate = false };
        var privateRoom = new ChatRoom("Private Room", user) { isPrivate = true };
        
        _context.ChatRooms.AddRange(publicRoom, privateRoom);
        await _context.SaveChangesAsync();
        
        // Act
        var result = await _chatService.GetAllPublicChatRooms();
        
        // Assert
        result.Should().HaveCount(1);
        result[0].Name.Should().Be("Public Room");
    }
    
    [Fact]
    public async Task SendMessage_ShouldAddMessageToRoom()
    {
        // Arrange
        var user = new User { Password = "",  Id = Guid.NewGuid(), Name = "Sender" };
        var room = new ChatRoom("Test Room", user);
        
        _context.Users.Add(user);
        _context.ChatRooms.Add(room);
        await _context.SaveChangesAsync();
        
        // Act
        var result = await _chatService.SendMessage(room.Id, user.Id, "Hello World");
        
        // Assert
        result.Should().BeTrue();
        
        var messages = await _chatService.GetRecentMessages(room.Id, 10);
        messages.Should().HaveCount(1);
        messages[0].Text.Should().Be("Hello World");
        messages[0].Sender.Id.Should().Be(user.Id);
    }
    
    [Fact]
    public async Task GetRecentMessages_ShouldReturnMessagesInDescendingOrder()
    {
        // Arrange
        var user = new User { Password = "",  Id = Guid.NewGuid(), Name = "User" };
        var room = new ChatRoom("Room", user);
        
        _context.Users.Add(user);
        _context.ChatRooms.Add(room);
        await _context.SaveChangesAsync();
        
        await _chatService.SendMessage(room.Id, user.Id, "First");
        await Task.Delay(10); // Ensure different timestamps
        await _chatService.SendMessage(room.Id, user.Id, "Second");
        await Task.Delay(10);
        await _chatService.SendMessage(room.Id, user.Id, "Third");
        
        // Act
        var result = await _chatService.GetRecentMessages(room.Id, 10);
        
        // Assert
        result.Should().HaveCount(3);
        result[0].Text.Should().Be("Third");
        result[1].Text.Should().Be("Second");
        result[2].Text.Should().Be("First");
    }
    
    [Fact]
    public async Task GetRecentMessages_ShouldRespectLimit()
    {
        // Arrange
        var user = new User { Password = "",  Id = Guid.NewGuid(), Name = "User" };
        var room = new ChatRoom("Room", user);
        
        _context.Users.Add(user);
        _context.ChatRooms.Add(room);
        await _context.SaveChangesAsync();
        
        for (int i = 0; i < 10; i++)
        {
            await _chatService.SendMessage(room.Id, user.Id, $"Message {i}");
        }
        
        // Act
        var result = await _chatService.GetRecentMessages(room.Id, 5);
        
        // Assert
        result.Should().HaveCount(5);
    }
    
    [Fact]
    public async Task AddUserToChatRoom_ShouldAddUser()
    {
        // Arrange
        var owner = new User { Password = "",  Id = Guid.NewGuid(), Name = "Owner" };
        var userToAdd = new User { Password = "",  Id = Guid.NewGuid(), Name = "NewUser" };
        var room = new ChatRoom("Room", owner);
        
        _context.Users.AddRange(owner, userToAdd);
        _context.ChatRooms.Add(room);
        await _context.SaveChangesAsync();
        
        // Act
        var result = await _chatService.AddUserToChatRoom(room.Id, userToAdd.Id);
        
        // Assert
        result.Should().BeTrue();
        
        var users = await _chatService.GetUsersInChatRoom(room.Id);
        users.Should().Contain(u => u.Id == userToAdd.Id);
    }
    
    [Fact]
    public async Task IsUserInChatRoom_ShouldReturnTrue_WhenUserInRoom()
    {
        // Arrange
        var user = new User { Password = "",  Id = Guid.NewGuid(), Name = "User" };
        var room = new ChatRoom("Room", user);
        room.Users.Add(user);
        
        _context.Users.Add(user);
        _context.ChatRooms.Add(room);
        await _context.SaveChangesAsync();
        
        // Act
        var result = await _chatService.IsUserInChatRoom(room.Id, user.Id);
        
        // Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task IsUserInChatRoom_ShouldReturnFalse_WhenUserNotInRoom()
    {
        // Arrange
        var owner = new User { Password = "",  Id = Guid.NewGuid(), Name = "Owner" };
        var otherUser = new User { Password = "",  Id = Guid.NewGuid(), Name = "Other" };
        var room = new ChatRoom("Room", owner);
        
        _context.Users.AddRange(owner, otherUser);
        _context.ChatRooms.Add(room);
        await _context.SaveChangesAsync();
        
        // Act
        var result = await _chatService.IsUserInChatRoom(room.Id, otherUser.Id);
        
        // Assert
        result.Should().BeFalse();
    }
    
    [Fact]
    public async Task SearchChatRooms_ShouldReturnMatchingRooms()
    {
        // Arrange
        var user = new User { Password = "",  Id = Guid.NewGuid(), Name = "User" };
        _context.Users.Add(user);
        
        _context.ChatRooms.AddRange(
            new ChatRoom("Gaming Chat", user),
            new ChatRoom("Random Stuff", user),
            new ChatRoom("Game Discussion", user)
        );
        await _context.SaveChangesAsync();
        
        // Act
        var result = await _chatService.SearchChatRooms("Gam");
        
        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(r => r.Name == "Gaming Chat");
        result.Should().Contain(r => r.Name == "Game Discussion");
    }
    
    [Fact]
    public async Task DeleteChatRoom_ShouldRemoveRoom()
    {
        // Arrange
        var user = new User { Password = "",  Id = Guid.NewGuid(), Name = "User" };
        var room = new ChatRoom("To Delete", user);
        
        _context.Users.Add(user);
        _context.ChatRooms.Add(room);
        await _context.SaveChangesAsync();
        
        // Act
        var deletedId = await _chatService.DeleteChatRoom(room.Id);
        
        // Assert
        deletedId.Should().Be(room.Id);
        
        var allRooms = await _chatService.GetAllChatRooms();
        allRooms.Should().NotContain(r => r.Id == room.Id);
    }
}