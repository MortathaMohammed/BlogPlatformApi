using BlogPlatformApi.Models;

namespace BlogPlatformApi.Services.Repository.IRepository;
public interface IReplyCommentsRepository : IGenericRejpository<ReplyComments>
{
    Task<IReadOnlyList<ReplyComments>> GetReplyCommentsByParent(string id);
    Task<IReadOnlyList<ReplyComments>> GetReplyCommentsByUser(string id);
}