using Microsoft.EntityFrameworkCore;
using Server.Shared.Models;
using Server.Shared.Models.Entities;

namespace Server.Data;

public class ChatterDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<ChatRoom> ChatRooms => Set<ChatRoom>();
    public DbSet<Message> Messages => Set<Message>();

    // Add this constructor for tests and dependency injection
    public ChatterDbContext(DbContextOptions<ChatterDbContext> options) : base(options)
    {
    }
    
    // Keep parameterless constructor for migrations/production
    public ChatterDbContext()
    {
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=chatter.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // modelBuilder.Entity<User>().HasMany(u => u.ChatRooms).WithOne(c => c.Owner);
        modelBuilder.Entity<ChatRoom>().HasOne(c => c.Owner).WithMany();
        modelBuilder.Entity<User>().HasMany(u => u.ChatRooms).WithMany(c => c.Users);


        modelBuilder.Entity<User>().HasMany(u => u.Messages).WithOne(m => m.Sender);
        modelBuilder.Entity<ChatRoom>().HasMany(c => c.Messages).WithOne(m => m.ChatRoom);
    }
}