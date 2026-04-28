namespace TechClient.Domain.Models;

public class ChatResponse
{
    public string SessionId { get; set; } = string.Empty;
    public string Reply { get; set; } = string.Empty;
    public string? Intent { get; set; }
}