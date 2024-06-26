namespace BlogPlatformApi.Models;
public class Tags
{
    public Guid Id { get; } = Guid.NewGuid();
    public required string TagName { get; set; }
}