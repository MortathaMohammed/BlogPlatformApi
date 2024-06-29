namespace BlogPlatformApi.Mapping.Posts;
public class UpdatePostDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
}