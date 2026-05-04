using Microsoft.EntityFrameworkCore;
using TechClient.Domain.Entities;
using TechClient.Infrastructure.Data.Mappings;

namespace TechClient.Infrastructure.Data;

public class TechClientDbContext : DbContext
{
    public TechClientDbContext(DbContextOptions<TechClientDbContext> options)
        : base(options) { }

    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Called> Calleds => Set<Called>();
    public DbSet<ConversationMessage> ConversationMessages => Set<ConversationMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ClientMapping());
        modelBuilder.ApplyConfiguration(new CalledMapping());
        modelBuilder.ApplyConfiguration(new ConversationMessageMapping());
    }
}