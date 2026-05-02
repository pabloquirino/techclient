using Microsoft.EntityFrameworkCore;
using TechClient.Domain.Entities;
using TechClient.Domain.Interfaces;
using TechClient.Infrastructure.Data;

namespace TechClient.Infrastructure.Repositories;

public class ConversationRepository(TechClientDbContext context) : IConversationRepository
{
    private readonly TechClientDbContext _context = context;

    public async Task AddMessageAsync(ConversationMessage message)
    {
        _context.ConversationMessages.Add(message);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ConversationMessage>> GetByCalledIdAsync(int calledId) =>
        await _context.ConversationMessages
            .Where(m => m.CalledId == calledId)
            .OrderBy(m => m.SentAt)
            .ToListAsync();
}