using BlogPlatformApi.Mapping.Posts;
using BlogPlatformApi.Models;

namespace BlogPlatformApi.Mapping.Tag;
public class GetTagsDto
{
    public Guid Id { get; set; }
    public string TagName { get; set; }
}