using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.Extensions;
using Server.Services.Interfaces;
using Server.Shared.Models;

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

    
    [HttpGet("chatrooms")]
    public async Task<ActionResult<List<ChatRoom>>> GetChatRooms()
    {
        return await _chatService.GetAllChatRooms();
    }
    
    [HttpGet("chatrooms/{id}")]
    public async Task<ActionResult<ChatRoom>> GetChatRoom(Guid id)
    {
        return await _chatService.GetChatRoomById(id);
    }

    [HttpPost("chatrooms/create")]
    public async Task<IActionResult> CreateChatRoom([FromBody] ChatRoomCreateReq req)
    {
        var user = await CurrentUser();
        var chatRoom = await _chatService.CreateChatRoom(req.Name, user);
        return Ok(chatRoom.Id);
    }
    
    
}

public record ChatRoomCreateReq(string Name);