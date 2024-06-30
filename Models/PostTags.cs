
namespace BlogPlatformApi.Models;
public class PostTags
{
    public Guid post_uid { get; set; }
    public Post Post { get; set; }

    public Guid tag_uid { get; set; }
    public Tags Tag { get; set; }

}