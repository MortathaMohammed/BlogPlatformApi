using BlogPlatformApi.Mapping.Posts;
using BlogPlatformApi.Models;

namespace BlogPlatformApi.Mapping.Tag;
public static class TagExtension
{
    public static GetTagsWithPostsDto AsTagPostsDto(this Tags tag)
    {
        return new GetTagsWithPostsDto
        {
            Id = tag.tag_uid,
            TagName = tag.tag_name,
            Posts = tag.posts.Select(post => post.AsDto()).ToList()
        };
    }
    public static GetTagsDto AsDto(this Tags tag)
    {
        return new GetTagsDto
        {
            Id = tag.tag_uid,
            TagName = tag.tag_name,
        };
    }
    public static Tags ToTagFromUpdateDto(this UpdateTagDto tag)
    {
        return new Tags
        {
            tag_uid = tag.Id,
            tag_name = tag.TagName,
        };
    }
    public static Tags ToTagFromCreateDto(this CreateTagDto tag)
    {
        return new Tags
        {
            tag_name = tag.TagName,
        };
    }

}