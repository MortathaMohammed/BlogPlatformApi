using BlogPlatformApi.Models;

namespace BlogPlatformApi.Mapping;
public class PostMapp
{
    public Guid post_uid { get; set; }
    public Guid user_uid { get; set; }
    public BlogUserMapp User { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime created_at { get; set; }

}