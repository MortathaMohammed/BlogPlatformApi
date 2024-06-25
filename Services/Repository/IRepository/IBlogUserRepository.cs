using BlogPlatformApi.Models;

namespace BlogPlatformApi.Services.Repository.IRepository;

public interface IBlogUserRepository : IGenericRejpository<BlogUser>
{
    Task<BlogUser> GetUserByUserName(string userName);

}