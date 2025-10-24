using Microsoft.EntityFrameworkCore;
using Server.Shared.Models;

namespace Server.Data;

public class ChatterDbContext : DbContext
{

    public DbSet<User> Users => Set<User>();
    public DbSet<ChatRoom> ChatRooms => Set<ChatRoom>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=chatter.db");
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().HasMany(u => u.ChatRooms).WithOne(c => c.Owner);

        modelBuilder.Entity<User>().HasMany(u => u.Messages).WithOne(m => m.Sender);
        
        modelBuilder.Entity<ChatRoom>().HasMany(c => c.Messages).WithOne(m => m.ChatRoom);
    }
}