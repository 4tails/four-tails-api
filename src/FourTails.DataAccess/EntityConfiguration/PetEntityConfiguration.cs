using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FourTails.Core.DomainModels;

namespace FourTails.DataAccess.EntityConfiguration;

public class PetEntityConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        // properties
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn();
        builder.Property(x => x.Age).IsRequired();
        builder.Property(x => x.Moniker).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Gender).IsRequired();
        builder.HasIndex(x => x.PetOwnerId).IsUnique();

        // entity
        builder.ToTable("Pets");
    }
}