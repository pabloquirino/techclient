using TechClient.Domain.Entities;

namespace TechClient.Domain.Interfaces;

public interface IClientRepository
{
    Task<Client?> GetByEmailAsync(string email);
    Task<Client> CreateAsync(Client client);
}