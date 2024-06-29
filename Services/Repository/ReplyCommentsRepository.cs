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
        VALUES(uuid_generate_v4(), @parent_comment_uid, @user_uid, @created_at, @content)
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql, replyComments, transaction: _dbTransaction);
        return result;
    }

    public async Task<int> DeleteAsync(Guid id)
    {
        var sql = "DELETE FROM reply_comments WHERE reply_uid = @reply_uid";
        var result = await _npgsqlConnection.ExecuteAsync(sql, new { reply_uid = id }, transaction: _dbTransaction);
        return result;
    }

    public async Task<IReadOnlyList<ReplyComments>> GetAllAsync()
    {
        var sql =
        """
            SELECT * FROM reply_comments 
            INNER JOIN users AS reply_users ON reply_users.user_uid = reply_comments.user_uid
            INNER JOIN comments ON reply_comments.parent_comment_uid = comments.comment_uid
            INNER JOIN users AS comment_users ON comment_users.user_uid = comments.user_uid
        """;

        var result = await _npgsqlConnection.QueryAsync<ReplyComments, BlogUser, Comment, BlogUser, ReplyComments>(
            sql,
            (reply, replyuser, comment, commentUser) =>
            {
                reply.user = replyuser;
                reply.comment = comment;
                reply.comment.user = commentUser;
                return reply;
            },
            splitOn: "user_uid,comment_uid,user_uid",
            transaction: _dbTransaction
        );

        return result.ToList();
    }

    public async Task<ReplyComments?> GetByIdAsync(Guid id)
    {
        var sql =
        """
            SELECT * FROM reply_comments
            INNER JOIN  users AS reply_users ON reply_users.user_uid = reply_comments.user_uid
            INNER JOIN  comments ON comments.comment_uid = reply_comments.parent_comment_uid
            INNER JOIN  users AS comment_users ON comment_users.user_uid = comments.user_uid
            WHERE reply_uid = @reply_uid
         """;
        var result = await _npgsqlConnection.QueryAsync<ReplyComments, BlogUser, Comment, BlogUser, ReplyComments>(sql,
        (reply, replyUser, comment, user) =>
        {
            reply.user = replyUser;
            reply.comment = comment;
            reply.comment.user = user;
            return reply;
        },
        new { reply_uid = id },
        splitOn: "user_uid, comment_uid, user_uid", transaction: _dbTransaction);

        var replyComments = result.FirstOrDefault();
        if (replyComments == null)
            return null!;

        return replyComments;
    }

    public async Task<IReadOnlyList<ReplyComments>> GetReplyCommentsByParent(Guid id)
    {
        var sql =
        """
            SELECT * FROM reply_comments 
            INNER JOIN users AS reply_users ON reply_users.user_uid = reply_comments.user_uid
            INNER JOIN comments ON reply_comments.parent_comment_uid = comments.comment_uid
            INNER JOIN users AS comment_users ON comment_users.user_uid = comments.user_uid
            WHERE reply_comments.parent_comment_id = @ParentId
        """;

        var result = await _npgsqlConnection.QueryAsync<ReplyComments, BlogUser, Comment, BlogUser, ReplyComments>(
            sql,
            (reply, replyuser, comment, commentUser) =>
            {
                reply.user = replyuser;
                reply.comment = comment;
                reply.comment.user = commentUser;
                return reply;
            },
            new { ParentId = id },
            splitOn: "user_uid,comment_uid,user_uid",
            transaction: _dbTransaction
        );

        return result.ToList();
    }

    public async Task<IReadOnlyList<ReplyComments>> GetReplyCommentsByUser(Guid id)
    {
        var sql =
        """
            SELECT * FROM reply_comments 
            INNER JOIN users AS reply_users ON reply_users.user_uid = reply_comments.user_uid
            INNER JOIN comments ON reply_comments.parent_comment_uid = comments.comment_uid
            INNER JOIN users AS comment_users ON comment_users.user_uid = comments.user_uid
            WHERE reply_comments.user_uid = @UserId
        """;

        var result = await _npgsqlConnection.QueryAsync<ReplyComments, BlogUser, Comment, BlogUser, ReplyComments>(
            sql,
            (reply, replyuser, comment, commentUser) =>
            {
                reply.user = replyuser;
                reply.comment = comment;
                reply.comment.user = commentUser;
                return reply;
            },
            new { UserId = id },
            splitOn: "user_uid,comment_uid,user_uid",
            transaction: _dbTransaction
        );

        return result.ToList();
    }

    public async Task<int> UpdateAsync(ReplyComments replyComments)
    {
        var sql =
        """
            UPDATE reply_comments SET
            content = @content
            WHERE reply_uid = @reply_uid
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql, new
        {
            replyComments.content,
            replyComments.reply_uid
        }, transaction: _dbTransaction);
        return result;
    }

}