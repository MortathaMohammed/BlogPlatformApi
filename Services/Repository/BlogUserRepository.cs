using System.Data;
using BlogPlatformApi.Models;
using BlogPlatformApi.Services.Repository.IRepository;
using Dapper;
using Npgsql;

namespace BlogPlatformApi.Services.Repository;

public class BlogUserRepository : IBlogUserRepository
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
        INSERT INTO users (user_uid, username, email, password_hash, created_at, bio)
        VALUES (uuid_generate_v4(), @username, @email, @password_hash, @created_at, @bio);
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql, user, transaction: _dbTransaction);
        return result;
    }

    public async Task<int> DeleteAsync(Guid id)
    {
        var sql = "DELETE FROM users WHERE user_uid = @user_uid";
        var result = await _npgsqlConnection.ExecuteAsync(sql, new { user_uid = id }, transaction: _dbTransaction);
        return result;
    }

    public async Task<IReadOnlyList<BlogUser>> GetAllAsync()
    {
        var sql = "SELECT  * FROM users";
        var result = await _npgsqlConnection.QueryAsync<BlogUser>(sql);
        if (result == null)
            return null!;

        return result.ToList();
    }

    public async Task<BlogUser?> GetByIdAsync(Guid id)
    {
        var sql = "SELECT * FROM users WHERE user_uid = @user_uid";
        var result = await _npgsqlConnection.QuerySingleOrDefaultAsync<BlogUser>(sql, new { user_uid = id }, transaction: _dbTransaction);
        if (result == null)
            return null!;
        return result;
    }

    public async Task<BlogUser> GetUserByUserName(string userName)
    {
        var sql = "SELECT * FROM users WHERE username = @username";
        var result = await _npgsqlConnection.QueryFirstOrDefaultAsync<BlogUser>(sql, new { username = userName }, transaction: _dbTransaction);
        if (result == null)
            return null!;

        return result;
    }

    public async Task<int> UpdateAsync(BlogUser user)
    {
        var sql =
        """
        UPDATE users SET  
        username = @username,
        email = @email,
        bio = @bio 
        WHERE user_uid = @user_uid
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql, new
        {
            user.username,
            user.email,
            user.bio,
            user.user_uid
        }, transaction: _dbTransaction);
        return result;
    }
}