using BlogPlatformApi.Models;

namespace BlogPlatformApi.Services.Repository.IRepository;
public interface ICommentRepository : IGenericRejpository<Comment>
{
    Task<IReadOnlyList<Comment>> GetCommentsByPostId(int id);
    Task<IReadOnlyList<Comment>> GetCommentsByUserId(int id);
}
