
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
            INSERT INTO posts(post_id, user_id, title, content, created_at)
            VALUES (@Id, @UserId, @Title, @Content, @CreatedAt)
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql, post, transaction: _dbTransaction);
        return result;
    }

    public async Task<int> DeleteAsync(string id)
    {
        var sql = "DELETE FROM posts WHERE post_id = @Id";
        var result = await _npgsqlConnection.ExecuteAsync(sql, new { Id = id }, transaction: _dbTransaction);
        return result;
    }

    public async Task<IReadOnlyList<Post>> GetAllAsync()
    {
        var sql = "SELECT * FROM posts";
        var result = await _npgsqlConnection.QueryAsync<Post>(sql);
        return result.ToList();
    }

    public async Task<Post?> GetByIdAsync(string id)
    {
        var sql = "SELECT * FROM posts WHERE post_id = @Id";
        var result = await _npgsqlConnection.QueryFirstOrDefaultAsync<Post>(sql, new { Id = id }, transaction: _dbTransaction);
        return result;
    }

    public async Task<Post> GetPostByTitle(string title)
    {
        var sql = "SELECT * FROM posts WHERE title = @Title";
        var result = await _npgsqlConnection.QueryFirstOrDefaultAsync<Post>(sql, new { Title = title }, transaction: _dbTransaction);
        return result!;
    }
    public async Task<IReadOnlyList<Post>> GetPostsByUser(string id)
    {
        var sql = """
        SELECT * FROM posts WHERE user_id = @UserId
        INNER JOIN user_blog ON posts.user_id = user_blog.user_id
        """;
        var result = await _npgsqlConnection.QueryAsync<Post, BlogUser, Post>(sql,
            (post, user) =>
            {
                post.User = user;
                return post;
            }, new { UserId = id }, splitOn: "UserId", transaction: _dbTransaction);
        return result.ToList();
    }
    public async Task<int> UpdateAsync(Post post)
    {
        var sql =
        """
            UPDATE posts SET
            title = @Title,
            content = @Content,
            created_at = @CreatedAt,
            WHERE id = @Id
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql, post, transaction: _dbTransaction);
        return result;
    }
}
