using BlogPlatformApi.Models;

namespace BlogPlatformApi.Services.Repository.IRepository;
public interface IReplyCommentsRepository : IGenericRejpository<ReplyComments>
{
    Task<IReadOnlyList<ReplyComments>> GetReplyCommentsByParent(Guid id);
    Task<IReadOnlyList<ReplyComments>> GetReplyCommentsByUser(Guid id);
}