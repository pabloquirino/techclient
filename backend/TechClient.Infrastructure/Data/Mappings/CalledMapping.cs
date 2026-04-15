using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechClient.Domain.Entities;

namespace TechClient.Infrastructure.Data.Mappings;

public class CalledMapping : IEntityTypeConfiguration<Called>
{
    public void Configure(EntityTypeBuilder<Called> builder)
    {
        builder.ToTable("Calleds");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Protocol).IsRequired().HasMaxLength(20);
        builder.HasIndex(x => x.Protocol).IsUnique();
        builder.Property(x => x.Description).IsRequired().HasMaxLength(1000);
        builder.Property(x => x.Status).IsRequired().HasMaxLength(20);

        builder.HasOne(x => x.Client)
               .WithMany(x => x.Calleds)
               .HasForeignKey(x => x.ClientId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}