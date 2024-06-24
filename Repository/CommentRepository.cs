
using System.Data;
using BlogPlatformApi.Models;
using BlogPlatformApi.Repository.IRepository;
using Dapper;
using Npgsql;

namespace BlogPlatformApi.Repository;

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
            INSERT INTO Comment(postid, bloguserid, timestamp)
            VALUES(@PostId, @BlogUserId, @Timestamp);
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
        var result = await _npgsqlConnection.QueryFirstOrDefaultAsync(sql, new { Id = id }, _dbTransaction);
        return result;
    }

    public async Task<int> UpdateAsync(Comment comment)
    {
        var sql =
        """
        UPDATE Comment SET
        postid = @PostId,
        bloguserid = @BlogUserId,
        timestamp = @Timestamp
        WHERE id = @Id
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql, comment, _dbTransaction);
        return result;
    }
}
