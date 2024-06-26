namespace BlogPlatformApi.Models;
public class BlogUser
{
    public Guid Id { get; } = Guid.NewGuid();
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public required string Bio { get; set; }
}