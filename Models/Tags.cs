namespace BlogPlatformApi.Models;
public class Tags
{
    public Guid tag_uid { get; set; }
    public string tag_name { get; set; }
    public List<Post> posts { get; set; } = [];
}