namespace BlogPlatformApi.Repository.IRepository;
public interface IUnitOfWork
{
    IBlogUserRepository Users { get; }
    ICommentRepository Comments { get; }
    IInteractionRepository Interactions { get; }
    IPostRepository Posts { get; }
    void Commit();
}