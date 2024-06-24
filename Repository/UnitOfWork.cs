
using System.Data;
using BlogPlatformApi.Repository.IRepository;

namespace BlogPlatformApi.Repository;
public class UnitOfWork : IUnitOfWork, IDisposable
{

    public IBlogUserRepository Users { get; }
    public ICommentRepository Comments { get; }
    public IInteractionRepository Interactions { get; }
    public IPostRepository Posts { get; }

    private readonly IDbTransaction _dbTransaction;

    public UnitOfWork(
                IDbTransaction dbTransaction,
                IBlogUserRepository userRepository,
                ICommentRepository commentRepository,
                IPostRepository postRepository,
                IInteractionRepository interactionRepository)
    {
        Users = userRepository;
        Comments = commentRepository;
        Interactions = interactionRepository;
        Posts = postRepository;
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
