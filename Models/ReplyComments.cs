namespace BlogPlatformApi.Models;
public class ReplyComments
{
    public Guid Id { get; } = Guid.NewGuid();
    public required string ParentCommentId { get; set; }
    public required string UserId { get; set; }
    public required string Content { get; set; }
    public DateTime CreatedAt { get; set; }
}