
using System.Data;
using BlogPlatformApi.Models;
using BlogPlatformApi.Services.Repository.IRepository;
using Dapper;
using Npgsql;

namespace BlogPlatformApi.Services.Repository;

public class CommentRepository : IGenericRejpository<Comment>, ICommentRepository
{
    private NpgsqlConnection _npgsqlConnection;
    private IDbTransaction _dbTransaction;

    public CommentRepository(NpgsqlConnection npgsqlConnection, IDbTransaction dbTransaction)
    {
        _npgsqlConnection = npgsqlConnection;
        _dbTransaction = dbTransaction;
    }

    public async Task<int> AddAsync(Comment comment)
    {
        var sql =
        """
            INSERT INTO comments(comment_id, post_id, user_id, content, created_at)
            VALUES(@Id, @PostId, @UserId, @Content, @CreatedAt);
        """;
        return await _npgsqlConnection.ExecuteAsync(sql, comment, transaction: _dbTransaction);
    }

    public async Task<int> DeleteAsync(string id)
    {
        var sql = "DELETE FROM comments WHERE comment_id = @Id";
        var result = await _npgsqlConnection.ExecuteAsync(sql, new { Id = id }, transaction: _dbTransaction);
        return result;
    }

    public async Task<IReadOnlyList<Comment>> GetAllAsync()
    {
        var sql = "SELECT * FROM comment";
        var result = await _npgsqlConnection.QueryAsync<Comment>(sql);
        return result.ToList();
    }

    public async Task<Comment?> GetByIdAsync(string id)
    {
        var sql = "SELECT * FROM comments WHERE comment_id = @Id";
        var result = await _npgsqlConnection.QueryFirstOrDefaultAsync<Comment>(sql, new { Id = id }, _dbTransaction);
        return result;
    }

    public async Task<IReadOnlyList<Comment>> GetCommentsByPostId(string id)
    {
        var sql = "SELECT * FROM comments WHERE post_id = @PostId";
        var result = await _npgsqlConnection.QueryAsync<Comment>(sql, new { PostId = id });
        return result.ToList();
    }

    public async Task<IReadOnlyList<Comment>> GetCommentsByUserId(string id)
    {
        var sql = "SELECT * FROM comments WHERE user_id = @UserId";
        var result = await _npgsqlConnection.QueryAsync<Comment>(sql, new { BlogUserId = id });
        return result.ToList();
    }

    public async Task<int> UpdateAsync(Comment comment)
    {
        var sql =
        """
        UPDATE comments SET
        post_id = @PostId,
        user_id = @UserId,
        content = @Content,
        created_at = @CreatedAt
        WHERE comment_id = @Id
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql, comment, _dbTransaction);
        return result;
    }
}
