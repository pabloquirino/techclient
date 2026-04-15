namespace TechClient.Domain.Entities;

public class Called
{
    public int Id { get; set; }
    public string Protocol { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "Open";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ClosedAt { get; set; }

    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;

    public ICollection<ConversationMessage> Messages { get; set; } = [];
}