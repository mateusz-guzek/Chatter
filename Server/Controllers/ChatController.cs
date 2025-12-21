using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.Extensions;
using Server.Services.Interfaces;
using Server.Shared.Models.Dtos;

namespace Server.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ChatController : ChatterControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(ChatterDbContext db, IChatService chatService) : base(db)
    {
        _chatService = chatService;
    }


    [HttpGet("rooms")]
    public async Task<ActionResult<List<ChatRoomDto>>> GetChatRooms()
    {
        return await _chatService.GetAllChatRooms();
    }

    [HttpPost("rooms/{roomId}/join")]
    public async Task<IActionResult> JoinRoom(Guid roomId)
    {
        var user = await CurrentUser();
        var result = await _chatService.JoinRoom(roomId, user.Id);
        return result switch
        {
            JoinRoomResult.Joined => Ok(),
            JoinRoomResult.AlreadyMember => Conflict(ErrorDto.Of("ALREADY_MEMBER", "User is already a member of this room")),
            JoinRoomResult.Forbidden => Forbid(),
            JoinRoomResult.RoomNotFound => NotFound(ErrorDto.Of("ROOM_NOT_FOUND", "Chat room not found")),
            JoinRoomResult.UserNotFound => NotFound(ErrorDto.Of("USER_NOT_FOUND", "User not found")),
            _ => StatusCode(500)
        };
    }

    [HttpPost("rooms/{roomId}/users/{userId}")]
    public async Task<IActionResult> AddUserToRoom(Guid roomId, Guid userId)
    {
        var performer = await CurrentUser();
        var result = await _chatService.AddUserToChatRoom(roomId, userId, performer.Id);
        return result switch
        {
            AddUserToRoomResult.Added => NoContent(),
            AddUserToRoomResult.AlreadyMember => Conflict(ErrorDto.Of("ALREADY_MEMBER", "User is already a member of this room")),
            AddUserToRoomResult.NotOwner => Forbid(),
            AddUserToRoomResult.Forbidden => Forbid(),
            AddUserToRoomResult.RoomNotFound => NotFound(ErrorDto.Of("ROOM_NOT_FOUND", "Chat room not found")),
            AddUserToRoomResult.UserNotFound => NotFound(ErrorDto.Of("USER_NOT_FOUND", "User not found")),
            _ => StatusCode(500)
        };
    }

    [HttpGet("rooms/{id}")]
    public async Task<ActionResult<ChatRoomDto>> GetChatRoom(Guid id)
    {
        return await _chatService.GetChatRoomById(id);
    }

    [HttpPost("rooms/create")]
    public async Task<IActionResult> CreateChatRoom([FromBody] CreateChatRoomRequest request)
    {
        var user = await CurrentUser();
        Console.WriteLine("User: " + user.Name + "");
        var chatRoom = await _chatService.CreateChatRoom(request.Name, user.Id);
        return Ok(chatRoom.Id);
    }


    [HttpPost("{chatRoomId}/messages")]
    public async Task<IActionResult> SendMessage(
        Guid chatRoomId,
        [FromBody] TextMessageRequest request)
    {
        var user = await CurrentUser();
        var isAllowed = await _chatService.IsUserInChatRoom(chatRoomId, user.Id);
        if (!isAllowed) return Unauthorized();
        var res = await _chatService.SendMessage(chatRoomId, user.Id, request.Message);
        if (!res) return StatusCode(500);
        return Ok(res);
    }

    [HttpGet("{chatRoomId}/messages")]
    public async Task<IActionResult> GetMessages(
        Guid chatRoomId,
        [FromQuery] int limit = 50,
        [FromQuery] DateTime? before = null)
    {
        var user = await CurrentUser();
        var isAllowed = await _chatService.IsUserInChatRoom(chatRoomId, user.Id);
        if (!isAllowed) return Unauthorized();

        // Normalize and clamp limit for safety
        limit = Math.Clamp(limit, 1, 200);

        var page = await _chatService.GetMessagesPage(chatRoomId, before, limit);
        return Ok(new ChatMessagePagedResponseDto(chatRoomId, page.Messages, page.NextCursor, page.HasMore));
    }
}

public record TextMessageRequest(string Message);

public record CreateChatRoomRequest(string Name);