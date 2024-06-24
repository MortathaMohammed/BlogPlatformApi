using BlogPlatformApi.Models;

namespace BlogPlatformApi.Repository.IRepository;

public interface IBlogUserRepository : IGenericRejpository<BlogUser>
{
    Task<BlogUser> GetUserByUserName(string userName);

}