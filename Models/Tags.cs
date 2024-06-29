namespace BlogPlatformApi.Models;
public class Tags
{
    public Guid tag_uid { get; set; }
    public required string tag_name { get; set; }
}