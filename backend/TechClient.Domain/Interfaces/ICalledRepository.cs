using TechClient.Domain.Entities;

namespace TechClient.Domain.Interfaces;

public interface ICalledRepository
{
    Task<Called> CreateAsync(Called called);
    Task<Called?> GetByProtocolAsync(string protocol);
    Task<IEnumerable<Called>> GetByClientIdAsync(int clientId);
}