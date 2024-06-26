namespace BlogPlatformApi.Models;
public class Post
{
    public Guid Id { get; } = Guid.NewGuid();
    public required string UserId { get; set; }
    public BlogUser? User { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}