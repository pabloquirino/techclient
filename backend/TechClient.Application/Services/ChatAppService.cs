using TechClient.Application.DTOs;
using TechClient.Domain.Exceptions;
using TechClient.Domain.Interfaces;
using TechClient.Domain.Models;

namespace TechClient.Application.Services;

public class ChatAppService(
    IChatService dialogflowService,
    IGenerativeAIService generativeAIService)
{
    private readonly IChatService _dialogflowService = dialogflowService;
    private readonly IGenerativeAIService _generativeAIService = generativeAIService;

    public async Task<ChatResponseDto> HandleMessageAsync(ChatRequestDto dto)
    {

        if (string.IsNullOrWhiteSpace(dto.Message))
            throw new BusinessException("Message cannot be empty.");

        if (dto.Message.Length > 500)
            throw new BusinessException("Message cannot exceed 500 characters.");

        var domainRequest = new ChatRequest
        {
            SessionId = dto.SessionId,
            Message = dto.Message
        };

        var dialogflowResponse = await _dialogflowService.SendMessageAsync(domainRequest);

        ChatResponse finalResponse;

        if (dialogflowResponse.IsFallback)
            finalResponse = await _generativeAIService.GenerateResponseAsync(domainRequest);
        else
            finalResponse = dialogflowResponse;

        return new ChatResponseDto
        {
            SessionId = finalResponse.SessionId,
            Reply = finalResponse.Reply,
            Source = dialogflowResponse.IsFallback ? "generative-ai" : "dialogflow"
        };
    }
}