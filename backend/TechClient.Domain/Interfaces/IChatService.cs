using TechClient.Domain.Models;

namespace TechClient.Domain.Interfaces;

public interface IChatService
{
    Task<ChatResponse> SendMessageAsync(ChatRequest request);
}