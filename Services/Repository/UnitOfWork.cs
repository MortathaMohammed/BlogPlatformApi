
using System.Data;
using BlogPlatformApi.Services.Repository.IRepository;

namespace BlogPlatformApi.Services.Repository;
public class UnitOfWork : IUnitOfWork, IDisposable
{

    public IBlogUserRepository Users { get; }
    public ICommentRepository Comments { get; }
    public IPostRepository Posts { get; }
    public IReplyCommentsRepository ReplyComments { get; }
    public ITagsRepository Tags { get; }
    private readonly IDbTransaction _dbTransaction;

    public UnitOfWork(
                IDbTransaction dbTransaction,
                IBlogUserRepository userRepository,
                ICommentRepository commentRepository,
                IPostRepository postRepository,
                IReplyCommentsRepository replyComments,
                ITagsRepository tags)
    {
        Users = userRepository;
        Comments = commentRepository;
        Posts = postRepository;
        ReplyComments = replyComments;
        Tags = tags;
        _dbTransaction = dbTransaction;
    }

    public void Commit()
    {
        try
        {
            _dbTransaction.Commit();
            _dbTransaction.Connection?.BeginTransaction();
        }
        catch
        {
            _dbTransaction.Rollback();
        }
    }

    public void Dispose()
    {
        _dbTransaction.Connection?.Close();
        _dbTransaction.Connection?.Dispose();
        _dbTransaction.Dispose();
    }


}
