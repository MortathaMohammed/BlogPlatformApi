namespace BlogPlatformApi.Mapping.User;
public class UpdateUserDto
{
    public Guid Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string Bio { get; set; }
}