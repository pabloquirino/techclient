using Microsoft.EntityFrameworkCore;
using TechClient.Domain.Entities;
using TechClient.Domain.Interfaces;
using TechClient.Infrastructure.Data;

namespace TechClient.Infrastructure.Repositories;

public class CalledRepository(TechClientDbContext context) : ICalledRepository
{
    private readonly TechClientDbContext _context = context;

    public async Task<Called> CreateAsync(Called called)
    {
        _context.Calleds.Add(called);
        await _context.SaveChangesAsync();
        return called;
    }

    public async Task<Called?> GetByProtocolAsync(string protocol) =>
        await _context.Calleds
            .Include(c => c.Client)
            .FirstOrDefaultAsync(c => c.Protocol == protocol);

    public async Task<IEnumerable<Called>> GetByClientIdAsync(int clientId) =>
        await _context.Calleds
            .Where(c => c.ClientId == clientId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
}