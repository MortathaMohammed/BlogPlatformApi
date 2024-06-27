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
        INSERT INTO users (user_uid, username, email, password_hash,created_at, bio)
        VALUES (uuid_generate_v4(), @Username, @Email, @PasswordHash, @CreatedAt, @Bio);
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql, user, transaction: _dbTransaction);
        return result;
    }

    public async Task<int> DeleteAsync(Guid id)
    {
        var sql = "DELETE FROM users WHERE user_uid = @Id";
        var result = await _npgsqlConnection.ExecuteAsync(sql, new { Id = id }, transaction: _dbTransaction);
        return result;
    }

    public async Task<IReadOnlyList<BlogUser>> GetAllAsync()
    {
        var sql = "SELECT  * FROM users";
        var rawResult = await _npgsqlConnection.QueryAsync(sql);
        if (rawResult == null)
            return null!;

        var result = rawResult.Select(x => new BlogUser
        {
            Id = (Guid)x.user_uid,
            Username = (string)x.username,
            Email = (string)x.email,
            Bio = (string)x.bio,
            PasswordHash = (string)x.password_hash,
            CreatedAt = (DateTime)x.created_at
        });

        return result.ToList();
    }

    public async Task<BlogUser?> GetByIdAsync(Guid id)
    {
        var sql = "SELECT * FROM users WHERE user_uid = @Id";
        var result = await _npgsqlConnection.QuerySingleOrDefaultAsync(sql, new { Id = id }, transaction: _dbTransaction);
        if (result == null)
            return null!;

        var user = new BlogUser
        {
            Id = (Guid)result!.user_uid,
            Username = (string)result.username,
            Email = (string)result.email,
            Bio = (string)result.bio,
            PasswordHash = (string)result.password_hash,
            CreatedAt = (DateTime)result.created_at
        };
        return user;
    }

    public async Task<BlogUser> GetUserByUserName(string userName)
    {
        var sql = "SELECT * FROM users WHERE username = @Username";
        var result = await _npgsqlConnection.QueryFirstOrDefaultAsync(sql, new { Username = userName }, transaction: _dbTransaction);
        if (result == null)
            return null!;

        var user = new BlogUser
        {
            Id = (Guid)result!.user_uid,
            Username = (string)result.username,
            Email = (string)result.email,
            Bio = (string)result.bio,
            PasswordHash = (string)result.password_hash,
            CreatedAt = (DateTime)result.created_at
        };
        return user;
    }

    public async Task<int> UpdateAsync(BlogUser user)
    {
        var sql =
        """
        UPDATE users SET  
        username = @Username,
        email = @Email,
        password_hash = @PasswordHash,
        created_at = @CreatedAt
        bio = @Bio 
        WHERE user_uid = @Id
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql, user, transaction: _dbTransaction);
        return result;
    }
}