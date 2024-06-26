namespace BlogPlatformApi.Models;
public class Comment
{
    public Guid Id { get;  }= Guid.NewGuid();
    public required string PostId { get; set; }
    public required string UserId { get; set; }
    public required string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}