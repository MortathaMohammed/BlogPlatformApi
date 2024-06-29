
namespace BlogPlatformApi.Models;
public class BlogUser
{
    public Guid user_uid { get; set; }
    public string username { get; set; }
    public string email { get; set; }
    public string password_hash { get; set; }
    public DateTime created_at { get; set; } = DateTime.UtcNow;
    public string bio { get; set; }
}