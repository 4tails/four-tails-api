using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FourTails.Core.DomainModels;

namespace FourTails.DataAccess.EntityConfiguration;

public class MessageEntityConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        // properties
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn();
        builder.Property(x => x.IncomingMessage).HasDefaultValue(null).IsRequired(false).HasMaxLength(2000);
        builder.HasIndex(x => x.MessageAuthorId).IsUnique();
        builder.Property(x => x.OutgoingMessage).IsRequired(true).HasMaxLength(2000);

        // entity
        builder.ToTable("Messages");
    }
}