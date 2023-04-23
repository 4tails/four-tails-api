using FourTails.Core.DomainModels;
using FourTails.DataAccess.EntityConfiguration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FourTails.DataAccess;

public class FTDBContext : IdentityDbContext<User>
{
    public FTDBContext() : base() {}
    public FTDBContext(DbContextOptions<FTDBContext> options) :base(options){}

    public DbSet<Message> Messages { get; set; }
    public DbSet<Pet> Pets { get; set; }
    public DbSet<User> IdentityUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        SetRelations(modelBuilder);
        SetTableProperties(modelBuilder);
    }

    public static void SetTableProperties(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new MessageEntityConfiguration());
        modelBuilder.ApplyConfiguration(new PetEntityConfiguration());
        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
    }

    public static void SetRelations(ModelBuilder modelBuilder)
    {
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