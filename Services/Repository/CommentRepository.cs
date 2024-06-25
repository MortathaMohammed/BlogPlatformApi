
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
            INSERT INTO Comment(postid, bloguserid, content, timestamp)
            VALUES(@PostId, @BlogUserId, @Content, @Timestamp);
        """;
        return await _npgsqlConnection.ExecuteAsync(sql, comment, transaction: _dbTransaction);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var sql = "DELETE FROM Comment WHERE id = @Id";
        var result = await _npgsqlConnection.ExecuteAsync(sql, new { Id = id }, transaction: _dbTransaction);
        return result;
    }

    public async Task<IReadOnlyList<Comment>> GetAllAsync()
    {
        var sql = "SELECT * FROM Comment";
        var result = await _npgsqlConnection.QueryAsync<Comment>(sql);
        return result.ToList();
    }

    public async Task<Comment?> GetByIdAsync(int id)
    {
        var sql = "SELECT * FROM Comment WHERE id = @Id";
        var result = await _npgsqlConnection.QueryFirstOrDefaultAsync<Comment>(sql, new { Id = id }, _dbTransaction);
        return result;
    }

    public async Task<IReadOnlyList<Comment>> GetCommentsByPostId(int id)
    {
        var sql = "SELECT * FROM Comment WHERE postid = @PostId";
        var result = await _npgsqlConnection.QueryAsync<Comment>(sql, new { PostId = id });
        return result.ToList();
    }

    public async Task<IReadOnlyList<Comment>> GetCommentsByUserId(int id)
    {
        var sql = "SELECT * FROM Comment WHERE bloguserid = @BlogUserId";
        var result = await _npgsqlConnection.QueryAsync<Comment>(sql, new { BlogUserId = id });
        return result.ToList();
    }

    public async Task<int> UpdateAsync(Comment comment)
    {
        var sql =
        """
        UPDATE Comment SET
        postid = @PostId,
        bloguserid = @BlogUserId,
        content = @Content,
        timestamp = @Timestamp
        WHERE id = @Id
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql, comment, _dbTransaction);
        return result;
    }
}
