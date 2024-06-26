namespace BlogPlatformApi.Services.Repository.IRepository;
public interface IUnitOfWork
{
    IBlogUserRepository Users { get; }
    ICommentRepository Comments { get; }
    IPostRepository Posts { get; }
    IReplyCommentsRepository ReplyComments { get; }
    ITagsRepository Tags { get; }
    void Commit();
}