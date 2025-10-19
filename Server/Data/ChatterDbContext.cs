using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Chatter.Server.Data;

public class ChatterDbContext : DbContext
{

    public DbSet<User> Users => Set<User>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=chatter.db");
    }
    
    
    
}