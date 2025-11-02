using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.Extensions;
using Server.Services.Interfaces;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ChatterControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(ChatterDbContext db, IAuthService authService) : base(db)
    {
        _authService = authService;
    }

    // [HttpPost("login")]
    // public async Task<ActionResult> Login([FromBody] LoginRequest request)
    // {
    //     try
    //     {
    //         var token = await _authService.Authenticate(request.Username, request.Password);
    //         if (token == null) return Unauthorized();
    //         return Ok(token);
    //     }
    //     catch (Exception e)
    //     {
    //         return BadRequest(e.Message);
    //     }
    // }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var success = await _authService.Register(request.Username, request.Password);
            if (!success) return BadRequest("Username already exists");
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize]
    [HttpPost("test")]
    public async Task<ActionResult> Test()
    {
        var user = await CurrentUser();
        return Ok(user);
    }
}

public record RegisterRequest(string Username, string Password);