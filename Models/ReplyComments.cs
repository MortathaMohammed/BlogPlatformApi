namespace BlogPlatformApi.Models;
public class ReplyComments
{
    public Guid reply_uid { get; set; }
    public Guid parent_comment_uid { get; set; }
    public Comment comment { get; set; }
    public Guid user_uid { get; set; }
    public BlogUser user { get; set; }
    public string content { get; set; }
    public DateTime created_at { get; set; } = DateTime.Now;
}