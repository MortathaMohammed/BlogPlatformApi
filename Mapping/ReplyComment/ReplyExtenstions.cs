using BlogPlatformApi.Mapping.Comments;
using BlogPlatformApi.Mapping.User;
using BlogPlatformApi.Models;

namespace BlogPlatformApi.Mapping.ReplyComment;
public static class ReplyExtenstions
{
    public static GetReplayCommentDto AsDto(this ReplyComments comment)
    {
        return new GetReplayCommentDto
        {
            Id = comment.reply_uid,
            Comment = comment.comment.AsDto(),
            User = comment.user.AsUserDto(),
            Content = comment.content,
            CreatedAt = comment.created_at
        };
    }
}