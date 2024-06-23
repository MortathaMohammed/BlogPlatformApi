namespace BlogPlatformApi.Models;
public class Post
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public required string Category { get; set; }
    public DateTime PublicationDate { get; set; } = DateTime.Now;
    public required string Tags { get; set; }
}