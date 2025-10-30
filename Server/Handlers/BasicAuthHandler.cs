using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Chatter.Server.Services;
using Chatter.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Server.Handlers;

public class BasicAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IAuthService _authService;
    public BasicAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IAuthService authService) : base(options, logger, encoder, clock)
    {
        _authService = authService;
    }

    public BasicAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, IAuthService authService) : base(options, logger, encoder)
    {
        _authService = authService;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            Logger.LogWarning("Authorization header is missing");
            return AuthenticateResult.NoResult();
        }

        try
        {
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            if(!authHeader.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase))
            {
                Logger.LogWarning("Invalid authentication scheme: {Scheme}", authHeader.Scheme);
                return AuthenticateResult.NoResult();
            }

            var credentialsBytes =  Convert.FromBase64String(authHeader.Parameter);
            var credentials = Encoding.UTF8.GetString(credentialsBytes).Split(':',2);
            var username = credentials[0];
            var password = credentials[1];
            
            var authenticated = await _authService.Authenticate(username, password);
            if (!authenticated)
            {
                Logger.LogWarning("Authentication failed for user: {Username}", username);
                return AuthenticateResult.Fail("");
            }
            var id = await _authService.GetId(username);
            var claims = new[] { new Claim(ClaimTypes.Name, username), new Claim(ClaimTypes.NameIdentifier, id) };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            Logger.LogInformation("User authenticated successfully: {Username}", username);
            return AuthenticateResult.Success(ticket);

        }
        catch (Exception e)
        {
            Logger.LogError(e, "Authentication error occurred");
            return AuthenticateResult.Fail("");
        }
    }
}