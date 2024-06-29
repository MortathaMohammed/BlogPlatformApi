namespace BlogPlatformApi.Mapping.User;
public class GetUserDto
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public required string Bio { get; set; }
}