using BlogPlatformApi.Mapping.Tag;
using BlogPlatformApi.Mapping.User;
using BlogPlatformApi.Models;

namespace BlogPlatformApi.Mapping.Posts;
public static class PostExtenstions
{
    public static GetPostDto AsDto(this Post post)
    {
        return new GetPostDto
        {
            Id = post.post_uid,
            User = post.user.AsUserDto(),
            Title = post.title,
            Content = post.content,
            CreatedAt = post.created_at,
            Tags = post.tags.Select(tag => tag.AsDto()).ToList()
        };
    }

    public static Post ToPostFromCreate(this CreatePostDto post)
    {
        return new Post
        {
            user_uid = post.UserId,
            title = post.Title,
            content = post.Content,
        };
    }

    public static Post ToPostFromUpdate(this UpdatePostDto post)
    {
        return new Post
        {
            post_uid = post.Id,
            title = post.Title,
            content = post.Content,
        };
    }
}