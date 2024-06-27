namespace BlogPlatformApi.Models;
public class ReplyComments
{
    public Guid Id { get; set; }
    public required Guid ParentCommentId { get; set; }
    public required Guid UserId { get; set; }
    public required string Content { get; set; }
    public DateTime CreatedAt { get; set; }
}