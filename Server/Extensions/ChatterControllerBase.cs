using System.Security.Claims;
using Chatter.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.Shared.Models;

namespace Server.Extensions;


public class ChatterControllerBase : ControllerBase
{
    private readonly ChatterDbContext _db;

    public ChatterControllerBase(ChatterDbContext db)
    {
        _db = db;
    }
    protected async Task<User> CurrentUser()
    {
        foreach (var userClaim in User.Claims)
        {
            Console.WriteLine(userClaim);
        }
        
            if (User == null || !User.Identity.IsAuthenticated)
                return null;

            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var guid = Guid.Parse(id);
            Console.WriteLine(guid);
            var user = await _db.Users.FindAsync(guid);

            return user;
    }
}