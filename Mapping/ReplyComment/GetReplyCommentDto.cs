using BlogPlatformApi.Mapping.Comments;
using BlogPlatformApi.Mapping.User;

namespace BlogPlatformApi.Mapping.ReplyComment;
public class GetReplayCommentDto
{
    public Guid Id { get; set; }
    public GetCommentDto Comment { get; set; }
    public UserDto User { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
}

