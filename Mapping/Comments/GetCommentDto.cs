using BlogPlatformApi.Mapping.User;

namespace BlogPlatformApi.Mapping.Comments;
public class GetCommentDto
{
    public Guid Id { get; set; }
    public UserDto User { get; set; }
    public string Content { get; set; }
}