using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechClient.Domain.Entities;

namespace TechClient.Infrastructure.Data.Mappings;

public class ConversationMessageMapping : IEntityTypeConfiguration<ConversationMessage>
{
    public void Configure(EntityTypeBuilder<ConversationMessage> builder)
    {
        builder.ToTable("ConversationMessages");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Content).IsRequired().HasMaxLength(2000);
        builder.Property(x => x.Sender).IsRequired().HasMaxLength(10);

        builder.HasOne(x => x.Called)
               .WithMany(x => x.Messages)
               .HasForeignKey(x => x.CalledId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}