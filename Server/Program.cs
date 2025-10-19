using Chatter.Server.Data;
using Chatter.Server.Services;
using Chatter.Server.Services.Interfaces;

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
        
        builder.Services.AddControllers();
        
        
        var app = builder.Build();
        
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ChatterDbContext>();
            db.Database.EnsureCreated();
        }
        
        app.MapControllers();
        

        app.Run();

    }
}