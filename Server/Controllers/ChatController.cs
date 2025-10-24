using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.Extensions;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChatController : ChatterControllerBase
{
    public ChatController(ChatterDbContext db) : base(db)
    {
    }
}