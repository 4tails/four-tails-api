using Microsoft.EntityFrameworkCore;
using FourTails.Core.DomainModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FourTails.DataAccess.EntityConfiguration;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(50);
        builder.ToTable("Roles");
    }
}