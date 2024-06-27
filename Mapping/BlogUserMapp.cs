namespace BlogPlatformApi.Mapping;
public class BlogUserMapp
{
    public Guid user_uid { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public required string password_hash { get; set; }
    public DateTime created_at { get; set; }
    public required string Bio { get; set; }
}