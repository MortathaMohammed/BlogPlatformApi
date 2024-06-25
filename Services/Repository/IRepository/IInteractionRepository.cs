using BlogPlatformApi.Models;

namespace BlogPlatformApi.Services.Repository.IRepository;

public interface IInteractionRepository : IGenericRejpository<Interaction>
{
    Task<IReadOnlyList<Interaction>> GetInteractionsByUserId(int id);
    Task<IReadOnlyList<Interaction>> GetInteractionsByPostId(int id);
}