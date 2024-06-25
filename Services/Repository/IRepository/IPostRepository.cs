using BlogPlatformApi.Models;

namespace BlogPlatformApi.Services.Repository.IRepository;

public interface IPostRepository : IGenericRejpository<Post>
{
    Task<Post> GetPostByTitle(string title);
}