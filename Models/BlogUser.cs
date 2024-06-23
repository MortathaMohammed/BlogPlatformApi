namespace BlogPlatformApi.Models;
public class BlogUser
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Bio { get; set; }
}