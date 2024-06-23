namespace BlogPlatformApi.Models;
public class Interaction
{
    public int Id { get; set; }
    public required int PostId { get; set; }
    public required int BlogUserId { get; set; }
    public required string Type { get; set; }

}