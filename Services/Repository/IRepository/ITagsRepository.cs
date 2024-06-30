using BlogPlatformApi.Models;

namespace BlogPlatformApi.Services.Repository.IRepository;
public interface ITagsRepository : IGenericRejpository<Tags>
{
    Task<IReadOnlyList<Tags>> GetTagsWithPostsByPostId(Guid id);
    Task<IReadOnlyList<Tags>> GetPostsByTagId(Guid id);
    Task<Tags> GetTagByName(string name);
}