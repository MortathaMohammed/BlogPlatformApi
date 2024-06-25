
using System.Data;
using BlogPlatformApi.Models;
using BlogPlatformApi.Services.Repository.IRepository;
using Dapper;
using Npgsql;

namespace BlogPlatformApi.Services.Repository;
public class InteractionRepository : IGenericRejpository<Interaction>, IInteractionRepository
{
    private NpgsqlConnection _npgsqlConnection;
    private IDbTransaction _dbTransaction;

    public InteractionRepository(NpgsqlConnection npgsqlConnection, IDbTransaction dbTransaction)
    {
        _npgsqlConnection = npgsqlConnection;
        _dbTransaction = dbTransaction;
    }

    public async Task<int> AddAsync(Interaction interaction)
    {
        var sql =
        """
            INSERT INTO Interaction(postid, bloguserid, type, timestamp)
            VALUES (@PostId, @BlogUserId, @Type, @Timestamp)
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql, interaction, transaction: _dbTransaction);
        return result;
    }

    public async Task<int> DeleteAsync(int id)
    {
        var sql = "DELETE FROM Interaction WHERE id = @Id";
        var result = await _npgsqlConnection.ExecuteAsync(sql, new { Id = id }, transaction: _dbTransaction);
        return result;
    }

    public async Task<IReadOnlyList<Interaction>> GetAllAsync()
    {
        var sql = "SELECT * FROM Interaction";
        var result = await _npgsqlConnection.QueryAsync<Interaction>(sql);
        return result.ToList();
    }

    public async Task<Interaction?> GetByIdAsync(int id)
    {
        var sql = "SELECT * FROM Interaction WHERE id = @Id";
        var result = await _npgsqlConnection.QueryFirstOrDefaultAsync<Interaction>(sql, new { Id = id }, transaction: _dbTransaction);
        return result;
    }

    public async Task<IReadOnlyList<Interaction>> GetInteractionsByPostId(int id)
    {
        var sql = "SELECT * FROM Interaction WHERE postid = @PostId";
        var result = await _npgsqlConnection.QueryAsync<Interaction>(sql, new { PostId = id }, transaction: _dbTransaction);
        return result.ToList();
    }

    public async Task<IReadOnlyList<Interaction>> GetInteractionsByUserId(int id)
    {
        var sql = "SELECT * FROM Interaction WHERE bloguserid = @BlogUserId";
        var result = await _npgsqlConnection.QueryAsync<Interaction>(sql, new { BlogUserId = id }, transaction: _dbTransaction);
        return result.ToList();
    }

    public async Task<int> UpdateAsync(Interaction interaction)
    {
        var sql =
        """
            UPDATE Interaction SET
            postid = @PostId,
            bloguserid = @BlogUserId,
            type = @Type,
            timestamp = @Timestamp
            WHERE id = @Id
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql, interaction, transaction: _dbTransaction);
        return result;
    }
}
