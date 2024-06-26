using System.Data;
using BlogPlatformApi.Models;
using BlogPlatformApi.Services.Repository.IRepository;
using Dapper;
using Npgsql;

namespace BlogPlatformApi.Services.Repository;
public class ReplyCommentsRepository : IReplyCommentsRepository
{
    private readonly IDbTransaction _dbTransaction;
    private readonly NpgsqlConnection _npgsqlConnection;

    public ReplyCommentsRepository(IDbTransaction dbTransaction, NpgsqlConnection npgsqlConnection)
    {
        _dbTransaction = dbTransaction;
        _npgsqlConnection = npgsqlConnection;
    }

    public async Task<int> AddAsync(ReplyComments replyComments)
    {
        var sql = """
        INSERT INTO reply_comments (reply_id, parent_comment_id, user_id, created_at, content)
        VALUES(@Id, @ParentCommentId, @UserId, @Content, @CreatedAt)
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql, replyComments, transaction: _dbTransaction);
        return result;
    }

    public async Task<int> DeleteAsync(string id)
    {
        var sql = "DELETE FROM reply_comments WHERE repy_id = @Id";
        var result = await _npgsqlConnection.ExecuteAsync(sql, new { Id = id });
        return result;
    }

    public async Task<IReadOnlyList<ReplyComments>> GetAllAsync()
    {
        var sql = "SELECT * FROM reply_comments";
        var result = await _npgsqlConnection.QueryAsync<ReplyComments>(sql, transaction: _dbTransaction);
        return result.ToList();
    }

    public async Task<ReplyComments?> GetByIdAsync(string id)
    {
        var sql = "SELECT * FROM reply_comments WHERE reply_id = @Id";
        var result = await _npgsqlConnection.QueryFirstOrDefaultAsync<ReplyComments>(sql, new { Id = id }, transaction: _dbTransaction);
        return result;
    }

    public async Task<IReadOnlyList<ReplyComments>> GetReplyCommentsByParent(string id)
    {
        var sql = "SELECT * FROM reply_comments WHERE parent_comment_id = @ParentCommentId";
        var result = await _npgsqlConnection.QueryAsync<ReplyComments>(sql, new { ParentCommentId = id }, transaction: _dbTransaction);
        return result.ToList();
    }

    public async Task<IReadOnlyList<ReplyComments>> GetReplyCommentsByUser(string id)
    {
        var sql = "SELECT * FROM reply_comments WHERE user_id = @UserId";
        var result = await _npgsqlConnection.QueryAsync<ReplyComments>(sql, new { UserId = id }, transaction: _dbTransaction);
        return result.ToList();
    }

    public async Task<int> UpdateAsync(ReplyComments replyComments)
    {
        var sql = """
        UPDATE reply_comments SET
        parent_commetn_id = @ParentCommentId,
        user_id = @UserId,
        created_at = @CreatedAt,
        content = @Content
        WHERE reply_id = @Id
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql, replyComments, transaction: _dbTransaction);
        return result;
    }

}