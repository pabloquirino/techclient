using Microsoft.EntityFrameworkCore;
using TechClient.Domain.Entities;
using TechClient.Domain.Interfaces;
using TechClient.Infrastructure.Data;

namespace TechClient.Infrastructure.Repositories;

public class ClientRepository(TechClientDbContext context) : IClientRepository
{
    private readonly TechClientDbContext _context = context;

    public async Task<Client?> GetByEmailAsync(string email) =>
        await _context.Clients.FirstOrDefaultAsync(c => c.Email == email);

    public async Task<Client> CreateAsync(Client client)
    {
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();
        return client;
    }
}