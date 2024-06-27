namespace BlogPlatformApi.Models;
public class Comment
{
    public Guid Id { get; set; }
    public required Guid PostId { get; set; }
    public required Guid UserId { get; set; }
    public required string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}