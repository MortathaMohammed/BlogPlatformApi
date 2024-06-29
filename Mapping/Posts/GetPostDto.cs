using BlogPlatformApi.Mapping.User;
using BlogPlatformApi.Models;

namespace BlogPlatformApi.Mapping.Posts;
public class GetPostDto
{
    public Guid Id { get; set; }
    public UserDto User { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
}