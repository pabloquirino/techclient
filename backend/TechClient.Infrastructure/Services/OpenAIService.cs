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
            Você é o assistente virtual do TechClient, sistema de suporte técnico da empresa.

            COMPORTAMENTO:
            - Seja empático, direto e profissional
            - Use linguagem simples, sem jargões técnicos desnecessários
            - Respostas curtas (máximo 3 parágrafos)
            - Use emojis com moderação para humanizar a conversa

            VOCÊ PODE:
            - Orientar sobre como abrir chamados
            - Explicar o status de um atendimento
            - Responder dúvidas gerais sobre o sistema
            - Orientar sobre recuperação de senha

            VOCÊ NÃO PODE:
            - Inventar protocolos, datas ou dados do sistema
            - Prometer prazos de resolução
            - Fornecer informações de outros clientes
            - Executar ações no sistema (apenas orientar)

            Se não souber responder com segurança, oriente o usuário a
            falar com um atendente humano digitando "falar com atendente".

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