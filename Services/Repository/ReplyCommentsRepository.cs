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
        INSERT INTO reply_comments (reply_uid, parent_comment_uid, user_uid, created_at, content)
        VALUES(uuid_generate_v4(), @ParentCommentId, @UserId, @Content, @CreatedAt)
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql, replyComments, transaction: _dbTransaction);
        return result;
    }

    public async Task<int> DeleteAsync(Guid id)
    {
        var sql = "DELETE FROM reply_comments WHERE repy_uid = @Id";
        var result = await _npgsqlConnection.ExecuteAsync(sql, new { Id = id }, transaction: _dbTransaction);
        return result;
    }

    public async Task<IReadOnlyList<ReplyComments>> GetAllAsync()
    {
        var sql = "SELECT * FROM reply_comments";
        var rawResult = await _npgsqlConnection.QueryAsync(sql);
        if (rawResult == null)
            return null!;


        var result = rawResult.Select(x => new ReplyComments
        {
            Id = (Guid)x.reply_uid,
            ParentCommentId = (Guid)x.parent_comment_uid,
            UserId = (Guid)x.user_uid,
            Content = (string)x.content,
            CreatedAt = (DateTime)x.created_at
        });
        return result.ToList();
    }

    public async Task<ReplyComments?> GetByIdAsync(Guid id)
    {
        var sql = "SELECT * FROM reply_comments WHERE reply_uid = @Id";
        var result = await _npgsqlConnection.QueryFirstOrDefaultAsync(sql, new { Id = id });
        if (result == null)
            return null!;

        var reply_comments = new ReplyComments
        {
            Id = (Guid)result.reply_uid,
            ParentCommentId = (Guid)result.parent_comment_uid,
            UserId = (Guid)result.user_uid,
            Content = (string)result.content,
            CreatedAt = (DateTime)result.created_at
        };
        return reply_comments;
    }

    public async Task<IReadOnlyList<ReplyComments>> GetReplyCommentsByParent(Guid id)
    {
        var sql = "SELECT * FROM reply_comments WHERE parent_comment_uid = @ParentCommentId";
        var rawResult = await _npgsqlConnection.QueryAsync(sql, new { ParentCommentId = id });
        if (rawResult == null)
            return null!;

        var result = rawResult.Select(x => new ReplyComments
        {
            Id = (Guid)x.reply_uid,
            ParentCommentId = (Guid)x.parent_comment_uid,
            UserId = (Guid)x.user_uid,
            Content = (string)x.content,
            CreatedAt = (DateTime)x.created_at
        });
        return result.ToList();
    }

    public async Task<IReadOnlyList<ReplyComments>> GetReplyCommentsByUser(Guid id)
    {
        var sql = "SELECT * FROM reply_comments WHERE user_uid = @UserId";
        var rawResult = await _npgsqlConnection.QueryAsync(sql, new { UserId = id });
        if (rawResult == null)
            return null!;

        var result = rawResult.Select(x => new ReplyComments
        {
            Id = (Guid)x.reply_uid,
            ParentCommentId = (Guid)x.parent_comment_uid,
            UserId = (Guid)x.user_uid,
            Content = (string)x.content,
            CreatedAt = (DateTime)x.created_at
        });
        return result.ToList();
    }

    public async Task<int> UpdateAsync(ReplyComments replyComments)
    {
        var sql = """
        UPDATE reply_comments SET
        parent_commetn_uid = @ParentCommentId,
        user_uid = @UserId,
        created_at = @CreatedAt,
        content = @Content
        WHERE reply_uid = @Id
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql, replyComments, transaction: _dbTransaction);
        return result;
    }

}