using System.Data;
using BlogPlatformApi.Models;
using BlogPlatformApi.Repository.IRepository;
using Dapper;
using Npgsql;

namespace BlogPlatformApi.Repository;

public class BlogUserRepository : IBlogUserRepository
{
    private NpgsqlConnection _npgsqlConnection;
    private IDbTransaction _dbTransaction;

    public BlogUserRepository(NpgsqlConnection npgsqlConnection, IDbTransaction dbTransaction)
    {
        _npgsqlConnection = npgsqlConnection;
        _dbTransaction = dbTransaction;
    }

    public async Task<int> AddAsync(BlogUser user)
    {
        var sql =
        """
        INSERT INTO BlogUser (username, email, password, bio)
        VALUES (@Username, @Email, @Password, @Bio);
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql, user, transaction: _dbTransaction);
        return result;
    }

    public async Task<int> DeleteAsync(int id)
    {
        var sql = "DELETE FROM BlogUser WHERE id = @Id";
        var result = await _npgsqlConnection.ExecuteAsync(sql, new { Id = id }, transaction: _dbTransaction);
        return result;
    }

    public async Task<IReadOnlyList<BlogUser>> GetAllAsync()
    {
        var sql = "SELECT * FROM BlogUser";
        var result = await _npgsqlConnection.QueryAsync<BlogUser>(sql);
        return result.ToList();
    }

    public async Task<BlogUser?> GetByIdAsync(int id)
    {
        var sql = "SELECT * FROM BlogUser WHERE id = @Id";
        var result = await _npgsqlConnection.QuerySingleOrDefaultAsync<BlogUser>(sql, new { Id = id }, transaction: _dbTransaction);
        return result;
    }

    public async Task<int> UpdateAsync(BlogUser user)
    {
        var sql =
        """
        UPDATE BlogUser SET  
        username = @Username,
        email = @Email,
        password = @Password,
        bio = @Bio
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql, user, transaction: _dbTransaction);
        return result;
    }
}