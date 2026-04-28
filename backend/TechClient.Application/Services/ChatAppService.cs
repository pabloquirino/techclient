using TechClient.Application.DTOs;
using TechClient.Domain.Interfaces;
using TechClient.Domain.Models;

namespace TechClient.Application.Services;

public class ChatAppService(IChatService chatService)
{
    private readonly IChatService _chatService = chatService;

    public async Task<ChatResponseDto> HandleMessageAsync(ChatRequestDto dto)
    {
        // DTO → Domain
        var request = new ChatRequest
        {
            SessionId = dto.SessionId,
            Message = dto.Message
        };

        var response = await _chatService.SendMessageAsync(request);

        // Domain → DTO
        return new ChatResponseDto
        {
            SessionId = response.SessionId,
            Reply = response.Reply,
            Intent = response.Intent
        };
    }
}