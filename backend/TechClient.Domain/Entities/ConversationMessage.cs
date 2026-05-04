namespace TechClient.Domain.Entities;

public class ConversationMessage
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string Sender { get; set; } = string.Empty; // "user" or "bot"
    public DateTime SentAt { get; set; } = DateTime.UtcNow;

    public int CalledId { get; set; }
    public Called Called { get; set; } = null!;
}