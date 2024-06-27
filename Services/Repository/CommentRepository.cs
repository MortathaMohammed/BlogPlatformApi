
using System.Data;
using BlogPlatformApi.Models;
using BlogPlatformApi.Services.Repository.IRepository;
using Dapper;
using Npgsql;

namespace BlogPlatformApi.Services.Repository;

public class CommentRepository : ICommentRepository
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
            INSERT INTO comments(comment_uid, post_uid, user_uid, content, created_at)
            VALUES(uuid_generate_v4(), @PostId, @UserId, @Content, @CreatedAt);
        """;
        return await _npgsqlConnection.ExecuteAsync(sql, comment, transaction: _dbTransaction);
    }

    public async Task<int> DeleteAsync(Guid id)
    {
        var sql = "DELETE FROM comments WHERE comment_uid = @Id";
        var result = await _npgsqlConnection.ExecuteAsync(sql, new { Id = id }, transaction: _dbTransaction);
        return result;
    }

    public async Task<IReadOnlyList<Comment>> GetAllAsync()
    {
        var sql = "SELECT * FROM comment";
        var rawResult = await _npgsqlConnection.QueryAsync(sql);
        if (rawResult == null)
            return null!;

        var result = rawResult.Select(x => new Comment
        {
            Id = (Guid)x.comment_uid,
            PostId = (Guid)x.post_uid,
            UserId = (Guid)x.user_uid,
            Content = (string)x.content,
            CreatedAt = (DateTime)x.created_at
        });
        return result.ToList();
    }

    public async Task<Comment?> GetByIdAsync(Guid id)
    {
        var sql = "SELECT * FROM comments WHERE comment_uid = @Id";
        var result = await _npgsqlConnection.QueryFirstOrDefaultAsync(sql, new { Id = id }, _dbTransaction);
        if (result == null)
            return null!;

        var comment = new Comment
        {
            Id = (Guid)result.comment_uid,
            PostId = (Guid)result.post_uid,
            UserId = (Guid)result.user_uid,
            Content = (string)result.content,
            CreatedAt = (DateTime)result.created_at
        };
        return comment;
    }

    public async Task<IReadOnlyList<Comment>> GetCommentsByPostId(Guid id)
    {
        var sql = "SELECT * FROM comments WHERE post_id = @PostId";
        var rawResult = await _npgsqlConnection.QueryAsync(sql, new { PostId = id });
        if (rawResult == null)
            return null!;

        var result = rawResult.Select(x => new Comment
        {
            Id = (Guid)x.comment_uid,
            PostId = (Guid)x.post_uid,
            UserId = (Guid)x.user_uid,
            Content = (string)x.content,
            CreatedAt = (DateTime)x.created_at
        });
        return result.ToList();
    }

    public async Task<IReadOnlyList<Comment>> GetCommentsByUserId(Guid id)
    {
        var sql = "SELECT * FROM comments WHERE user_uid = @UserId";
        var rawResult = await _npgsqlConnection.QueryAsync(sql, new { UserId = id });
        if (rawResult == null)
            return null!;

        var result = rawResult.Select(x => new Comment
        {
            Id = (Guid)x.comment_uid,
            PostId = (Guid)x.post_uid,
            UserId = (Guid)x.user_uid,
            Content = (string)x.content,
            CreatedAt = (DateTime)x.created_at
        });
        return result.ToList();
    }

    public async Task<int> UpdateAsync(Comment comment)
    {
        var sql =
        """
        UPDATE comments SET
        post_uid = @PostId,
        user_uid = @UserId,
        content = @Content,
        created_at = @CreatedAt
        WHERE comment_uid = @Id
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql, comment, _dbTransaction);
        return result;
    }
}
