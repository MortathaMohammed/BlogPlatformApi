using BlogPlatformApi.Mapping.Posts;
using BlogPlatformApi.Models;

namespace BlogPlatformApi.Mapping.Tag;
public class GetTagsWithPostsDto
{
    public Guid Id { get; set; }
    public string TagName { get; set; }
    public List<GetPostDto> Posts { get; set; }
}