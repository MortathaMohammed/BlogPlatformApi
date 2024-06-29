namespace BlogPlatformApi.Models;
public class Comment
{
    public Guid comment_uid { get; set; }
    public Guid post_uid { get; set; }
    public Guid user_uid { get; set; }
    public BlogUser user { get; set; }
    public string content { get; set; }
    public DateTime created_at { get; set; } = DateTime.Now;
}