namespace BlogPlatformApi.Mapping.Posts;
public class CreatePostDto
{
    public Guid UserId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
}