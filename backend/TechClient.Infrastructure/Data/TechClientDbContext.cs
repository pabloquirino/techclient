using Microsoft.EntityFrameworkCore;
using TechClient.Domain.Entities;

namespace TechClient.Infrastructure.Data;

public class TechClientDbContext : DbContext
{
    public TechClientDbContext(DbContextOptions<TechClientDbContext> options)
        : base(options) { }

    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Called> Calleds => Set<Called>();
}