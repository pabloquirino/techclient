using TechClient.Domain.Models;

namespace TechClient.Domain.Interfaces;

public interface IGenerativeAIService
{
    Task<ChatResponse> GenerateResponseAsync(ChatRequest request);
}