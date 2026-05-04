namespace TechClient.Application.DTOs;

public class CalledResponseDto
{
    public string Protocol { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string ClientEmail { get; set; } = string.Empty;
}