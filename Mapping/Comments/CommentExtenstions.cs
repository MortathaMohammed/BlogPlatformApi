using BlogPlatformApi.Mapping.User;
using BlogPlatformApi.Models;

namespace BlogPlatformApi.Mapping.Comments;
public static class CommentExtenstions
{
    public static GetCommentDto AsDto(this Comment comment)
    {
        return new GetCommentDto
        {
            Id = comment.comment_uid,
            User = comment.user.AsUserDto(),
            Content = comment.content
        };
    }

    public static Comment ToCommentFromCreate(this CreateCommentDto comment)
    {
        return new Comment
        {
            post_uid = comment.PostId,
            user_uid = comment.UserId,
            content = comment.Content,
        };
    }

    public static Comment ToCommentFromUpdate(this UpdateCommentDto comment)
    {
        return new Comment
        {
            comment_uid = comment.Id,
            content = comment.Content
        };
    }
}