using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Server.Shared.Models;

namespace Server.Extensions;


public class ChatterControllerBase : ControllerBase
{
    protected ReqUser CurrentUser
    {
        get
        {
            if (User == null || !User.Identity.IsAuthenticated)
                return null;

            return new ReqUser(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, User.Identity.Name);
        }
    }
}