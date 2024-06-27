using BlogPlatformApi.Models;

namespace BlogPlatformApi.Services.Repository.IRepository;
public interface ICommentRepository : IGenericRejpository<Comment>
{
    Task<IReadOnlyList<Comment>> GetCommentsByPostId(Guid id);
    Task<IReadOnlyList<Comment>> GetCommentsByUserId(Guid id);
}
