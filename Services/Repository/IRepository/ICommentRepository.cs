using BlogPlatformApi.Models;

namespace BlogPlatformApi.Services.Repository.IRepository;
public interface ICommentRepository : IGenericRejpository<Comment>
{
    Task<IReadOnlyList<Comment>> GetCommentsByPostId(string id);
    Task<IReadOnlyList<Comment>> GetCommentsByUserId(string id);
}
