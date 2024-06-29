
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
            VALUES(uuid_generate_v4(), @post_uid, @user_uid, @content, @created_at);
        """;
        return await _npgsqlConnection.ExecuteAsync(sql, comment, transaction: _dbTransaction);
    }

    public async Task<int> DeleteAsync(Guid id)
    {
        var sql = "DELETE FROM comments WHERE comment_uid = @comment_uid";
        var result = await _npgsqlConnection.ExecuteAsync(sql, new { comment_uid = id }, transaction: _dbTransaction);
        return result;
    }

    public async Task<IReadOnlyList<Comment>> GetAllAsync()
    {
        var sql = """
        SELECT * FROM comments
        INNER JOIN users ON comments.user_uid = users.user_uid
        """;
        var result = await _npgsqlConnection.QueryAsync<Comment, BlogUser, Comment>(sql,
        (comment, user) =>
        {
            comment.user = user;
            return comment;
        },
        splitOn: "user_uid", transaction: _dbTransaction);
        if (result == null)
            return null!;

        return result.ToList();
    }

    public async Task<Comment?> GetByIdAsync(Guid id)
    {
        var sql =
        """
            SELECT * FROM comments
            INNER JOIN users ON comments.user_uid = users.user_uid
            WHERE comments.comment_uid = comment_uid
        """;
        var result = await _npgsqlConnection.QueryAsync<Comment, BlogUser, Comment>(sql,
        (comment, user) =>
        {
            comment.user = user;
            return comment;
        },
        new { comment_uid = id },
        splitOn: "user_uid", transaction: _dbTransaction);

        var comment = result.FirstOrDefault();
        if (comment == null)
            return null!;

        return comment;
    }

    public async Task<IReadOnlyList<Comment>> GetCommentsByPostId(Guid id)
    {
        var sql =
        """
            SELECT * FROM comments  
            INNER JOIN users ON comments.user_uid = users.user_uid
            WHERE post_uid = @post_uid
        """;
        var result = await _npgsqlConnection.QueryAsync<Comment, BlogUser, Comment>(sql,
        (comment, user) =>
        {
            comment.user = user;
            return comment;
        },
        new { post_uid = id },
        splitOn: "user_uid", transaction: _dbTransaction);
        if (result == null)
            return null!;

        return result.ToList();
    }

    public async Task<IReadOnlyList<Comment>> GetCommentsByUserId(Guid id)
    {
        var sql =
        """
            SELECT * FROM comments  
            INNER JOIN users ON comments.user_uid = users.user_uid
            WHERE comments.user_uid = @user_uid
        """;
        var result = await _npgsqlConnection.QueryAsync<Comment, BlogUser, Comment>(sql,
        (comment, user) =>
        {
            comment.user = user;
            return comment;
        },
        new { user_uid = id },
        splitOn: "user_uid", transaction: _dbTransaction);
        if (result == null)
            return null!;

        return result.ToList();
    }

    public async Task<int> UpdateAsync(Comment comment)
    {
        var sql =
        """
            UPDATE comments SET
            content = @content,
            WHERE comment_uid = @comment_uid
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql,
        new
        {
            comment.comment_uid,
            comment.content
        }, transaction: _dbTransaction);
        return result;
    }
}
