using BlogPlatformApi.Models;

namespace BlogPlatformApi.Services.Repository.IRepository;

public interface IPostRepository : IGenericRejpository<Post>
{
    Task<Post> GetPostByTitle(string title);
    Task<IReadOnlyList<Post>> GetPostsByUser(Guid id);
    Task<int> AddPostTagId(PostTags postTags);
    Task<int> DeletePostTagId(Guid postId, Guid tagId);
}