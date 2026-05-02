using TechClient.Application.DTOs;
using TechClient.Domain.Entities;
using TechClient.Domain.Interfaces;

namespace TechClient.Application.Services;

public class CalledAppService(
    ICalledRepository calledRepository,
    IClientRepository clientRepository,
    IConversationRepository conversationRepository)
{
    private readonly ICalledRepository _calledRepository = calledRepository;
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IConversationRepository _conversationRepository = conversationRepository;

    public async Task<CalledResponseDto> OpenCalledAsync(OpenCalledDto dto)
    {
        var client = await _clientRepository.GetByEmailAsync(dto.ClientEmail)
            ?? throw new Exception($"Client not found: {dto.ClientEmail}");

        var called = new Called
        {
            Protocol = GenerateProtocol(),
            Description = dto.Description,
            Status = "Open",
            ClientId = client.Id
        };

        await _calledRepository.CreateAsync(called);

        return new CalledResponseDto
        {
            Protocol = called.Protocol,
            Description = called.Description,
            Status = called.Status,
            CreatedAt = called.CreatedAt,
            ClientEmail = client.Email
        };
    }

    public async Task<CalledResponseDto> GetByProtocolAsync(string protocol)
    {
        var called = await _calledRepository.GetByProtocolAsync(protocol)
            ?? throw new Exception($"Called not found: {protocol}");

        return new CalledResponseDto
        {
            Protocol = called.Protocol,
            Description = called.Description,
            Status = called.Status,
            CreatedAt = called.CreatedAt,
            ClientEmail = called.Client.Email
        };
    }

    public async Task SaveMessageAsync(int calledId, string content, string sender)
    {
        var message = new ConversationMessage
        {
            CalledId = calledId,
            Content = content,
            Sender = sender
        };

        await _conversationRepository.AddMessageAsync(message);
    }

    private static string GenerateProtocol() =>
        $"TK-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..4].ToUpper()}";
}