
using System.Data;
using BlogPlatformApi.Models;
using BlogPlatformApi.Repository.IRepository;
using Dapper;
using Npgsql;

namespace BlogPlatformApi.Repository;
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
            INSERT INTO Interaction(postid, bloguserid, type)
            VALUES (@PostId, @BlogUserId, @Type)
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
        var result = await _npgsqlConnection.QueryFirstOrDefaultAsync(sql, new { Id = id }, transaction: _dbTransaction);
        return result;
    }

    public async Task<int> UpdateAsync(Interaction interaction)
    {
        var sql =
        """
            UPDATE Interaction SET
            postid = @PostId,
            bloguserid = @BlogUserId,
            type = @Type
            WHERE id = @Id
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql, interaction, transaction: _dbTransaction);
        return result;
    }
}
