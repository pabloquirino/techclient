using Microsoft.Extensions.Configuration;
using OpenAI.Chat;
using TechClient.Domain.Interfaces;
using TechClient.Domain.Models;

namespace TechClient.Infrastructure.Services;

public class OpenAIService : IGenerativeAIService
{
    private readonly ChatClient _chatClient;

    public OpenAIService(IConfiguration configuration)
    {
        var apiKey = configuration["OpenAI:ApiKey"]!;
        var model = configuration["OpenAI:Model"]!;
        _chatClient = new ChatClient(model, apiKey);
    }

    public async Task<ChatResponse> GenerateResponseAsync(ChatRequest request)
    {
        var systemPrompt = """
            Você é o assistente virtual do TechClient, um sistema de suporte técnico.
            Seu tom é profissional, direto e empático.
            Você ajuda usuários a resolver problemas técnicos, abrir chamados e consultar status.
            Nunca invente protocolos ou dados do sistema.
            Se não souber a resposta, oriente o usuário a falar com um atendente humano.
            Responda sempre em português brasileiro.
            """;

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(systemPrompt),
            new UserChatMessage(request.Message)
        };

        var response = await _chatClient.CompleteChatAsync(messages);

        return new ChatResponse
        {
            SessionId = request.SessionId,
            Message = request.Message,
            Reply = response.Value.Content[0].Text,
            IsFallback = false
        };
    }
}