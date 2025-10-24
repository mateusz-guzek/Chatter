using Chatter.Server.Services;
using Chatter.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Server.Data;
using Server.Handlers;
using Server.Services;

namespace Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddMemoryCache();
        builder.Services.AddDbContext<ChatterDbContext>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        
        builder.Services.AddAuthentication("BasicAuthentication")
            .AddScheme<AuthenticationSchemeOptions, BasicAuthHandler>("BasicAuthentication", null);
        builder.Services.AddAuthorization();
        
        builder.Services.AddControllers();
        
        
        var app = builder.Build();
        
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ChatterDbContext>();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }
        
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.MapControllers();
        

        app.Run();

    }
}