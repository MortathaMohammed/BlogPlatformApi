using System.Data;
using BlogPlatformApi.Models;
using BlogPlatformApi.Services.Repository.IRepository;
using Dapper;
using Npgsql;

namespace BlogPlatformApi.Services.Repository;

public class BlogUserRepository : IBlogUserRepository, IGenericRejpository<BlogUser>
{
    private readonly NpgsqlConnection _npgsqlConnection;
    private readonly IDbTransaction _dbTransaction;

    public BlogUserRepository(NpgsqlConnection npgsqlConnection, IDbTransaction dbTransaction)
    {
        _npgsqlConnection = npgsqlConnection;
        _dbTransaction = dbTransaction;
    }

    public async Task<int> AddAsync(BlogUser user)
    {
        var sql =
        """
        INSERT INTO user_blog (user_id, username, email, password_hash,created_at, bio)
        VALUES (@Id, @Username, @Email, @PasswordHash, @CreatedAt, @Bio);
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql, user, transaction: _dbTransaction);
        return result;
    }

    public async Task<int> DeleteAsync(string id)
    {
        var sql = "DELETE FROM user_blog WHERE user_id = @Id";
        var result = await _npgsqlConnection.ExecuteAsync(sql, new { Id = id }, transaction: _dbTransaction);
        return result;
    }

    public async Task<IReadOnlyList<BlogUser>> GetAllAsync()
    {
        var sql = "SELECT * FROM user_blog";
        var result = await _npgsqlConnection.QueryAsync<BlogUser>(sql);
        return result.ToList();
    }

    public async Task<BlogUser?> GetByIdAsync(string id)
    {
        var sql = "SELECT * FROM user_blog WHERE user_id = @Id";
        var result = await _npgsqlConnection.QuerySingleOrDefaultAsync<BlogUser>(sql, new { Id = id }, transaction: _dbTransaction);
        return result;
    }

    public async Task<BlogUser> GetUserByUserName(string userName)
    {
        var sql = "SELECT * FROM user_blog WHERE username = @Username";
        var result = await _npgsqlConnection.QueryFirstOrDefaultAsync<BlogUser>(sql, new { Username = userName }, transaction: _dbTransaction);
        return result!;
    }

    public async Task<int> UpdateAsync(BlogUser user)
    {
        var sql =
        """
        UPDATE user_blog SET  
        username = @Username,
        email = @Email,
        password_hash = @PasswordHash,
        created_at = @CreatedAt
        bio = @Bio 
        WHERE user_id = @Id
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql, user, transaction: _dbTransaction);
        return result;
    }
}