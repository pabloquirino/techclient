using TechClient.Domain.Entities;

namespace TechClient.Domain.Interfaces;

public interface IConversationRepository
{
    Task AddMessageAsync(ConversationMessage message);
    Task<IEnumerable<ConversationMessage>> GetByCalledIdAsync(int calledId);
}