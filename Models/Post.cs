using BlogPlatformApi.Mapping;

namespace BlogPlatformApi.Models;
public class Post
{
    public Guid Id { get; set; }
    public required Guid UserId { get; set; }
    public BlogUserMapp? User { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}