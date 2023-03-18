using Microsoft.EntityFrameworkCore;
using FourTails.Core.DomainModels;
using FourTails.DataAccess.EntityConfiguration;

namespace FourTails.DataAccess;

public class FTDBContext : DbContext
{
    private readonly DbContextOptions<FTDBContext> _context;

    public FTDBContext(DbContextOptions<FTDBContext> context)
    { 
        _context = context;
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Pet> Pets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // set properties
        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
        modelBuilder.ApplyConfiguration(new MessageEntityConfiguration());
        modelBuilder.ApplyConfiguration(new PetEntityConfiguration());
        
        // set relations
        modelBuilder
        .Entity<Message>()
        .HasOne<User>(u => u.MessageAuthor)
        .WithMany(m => m.Messages)
        .HasForeignKey(u => u.MessageAuthorId);

        modelBuilder
        .Entity<Pet>()
        .HasOne<User>(u => u.PetOwner)
        .WithMany(p => p.Pets)
        .HasForeignKey(u => u.PetOwnerId);
    }
}