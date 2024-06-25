namespace BlogPlatformApi.Models;
public class Comment
{
    public int Id { get; set; }
    public required int PostId { get; set; }
    public required int BlogUserId { get; set; }
    public string? Content { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
}