
using System.Data;
using BlogPlatformApi.Models;
using BlogPlatformApi.Services.Repository.IRepository;
using Dapper;
using Npgsql;

namespace BlogPlatformApi.Services.Repository;
public class PostRepository : IGenericRejpository<Post>, IPostRepository
{
    private readonly NpgsqlConnection _npgsqlConnection;
    private readonly IDbTransaction _dbTransaction;

    public PostRepository(NpgsqlConnection npgsqlConnection, IDbTransaction dbTransaction)
    {
        _npgsqlConnection = npgsqlConnection;
        _dbTransaction = dbTransaction;
    }

    public async Task<int> AddAsync(Post post)
    {
        var sql =
        """
            INSERT INTO Post(title, content, category, publicationdate,tags)
            VALUES (@Title, @Content, @Category, @PublicationDate, @Tags)
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql, post, transaction: _dbTransaction);
        return result;
    }

    public async Task<int> DeleteAsync(int id)
    {
        var sql = "DELETE FROM Post WHERE id = @Id";
        var result = await _npgsqlConnection.ExecuteAsync(sql, new { Id = id }, transaction: _dbTransaction);
        return result;
    }

    public async Task<IReadOnlyList<Post>> GetAllAsync()
    {
        var sql = "SELECT * FROM Post";
        var result = await _npgsqlConnection.QueryAsync<Post>(sql);
        return result.ToList();
    }

    public async Task<Post?> GetByIdAsync(int id)
    {
        var sql = "SELECT * FROM Post WHERE id = @Id";
        var result = await _npgsqlConnection.QueryFirstOrDefaultAsync<Post>(sql, new { Id = id }, transaction: _dbTransaction);
        return result;
    }

    public async Task<Post> GetPostByTitle(string title)
    {
        var sql = "SELECT * FROM Post WHERE title = @Title";
        var result = await _npgsqlConnection.QueryFirstOrDefaultAsync<Post>(sql, new { Title = title }, transaction: _dbTransaction);
        return result!;
    }

    public async Task<int> UpdateAsync(Post post)
    {
        var sql =
        """
            UPDATE Post SET
            title = @Title,
            content = @Content,
            category = @Category,
            publicationdate = @PublicationDate,
            tags = @Tags
            WHERE id = @Id
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql, post, transaction: _dbTransaction);
        return result;
    }
}
