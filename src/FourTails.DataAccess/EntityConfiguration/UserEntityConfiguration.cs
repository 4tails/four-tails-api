using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FourTails.Core.DomainModels;

namespace FourTails.DataAccess.EntityConfiguration;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // properties
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn();
        builder.Property(x => x.Address).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Age).IsRequired();
        builder.Property(x => x.CreatedBy).IsRequired();
        builder.Property(x => x.CreatedOn).IsRequired();
        builder.Property(x => x.Email).IsRequired().HasMaxLength(100);
        builder.Property(x => x.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(x => x.LastName).IsRequired().HasMaxLength(50);
        builder.Property(x => x.UpdatedBy).HasDefaultValue(null);
        builder.Property(x => x.UpdatedOn).HasDefaultValue(null);
        builder.Property(x => x.Role).IsRequired();

        // relations
        builder.Property(x => x.Messages);
        builder.Property(x => x.Pets);

        // entity
        builder.ToTable("Users");
    }
}