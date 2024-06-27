
using System.Data;
using BlogPlatformApi.Models;
using BlogPlatformApi.Services.Repository.IRepository;
using Dapper;
using Npgsql;

namespace BlogPlatformApi.Services.Repository;
public class PostRepository : IPostRepository
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
            INSERT INTO posts(post_uid, user_uid, title, content, created_at)
            VALUES (uuid_generate_v4(), @UserId, @Title, @Content, @CreatedAt)
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql, post, transaction: _dbTransaction);
        return result;
    }

    public async Task<int> DeleteAsync(Guid id)
    {
        var sql = "DELETE FROM posts WHERE post_uid = @Id";
        var result = await _npgsqlConnection.ExecuteAsync(sql, new { Id = id }, transaction: _dbTransaction);
        return result;
    }

    public async Task<IReadOnlyList<Post>> GetAllAsync()
    {
        var sql = "SELECT * FROM posts";
        var rawResult = await _npgsqlConnection.QueryAsync(sql);
        if (rawResult == null)
            return null!;

        var result = rawResult.Select(x => new Post
        {
            Id = (Guid)x.post_uid,
            UserId = (Guid)x.user_uid,
            Title = (string)x.title,
            Content = (string)x.content,
            CreatedAt = (DateTime)x.created_at
        });
        return result.ToList();
    }

    public async Task<Post?> GetByIdAsync(Guid id)
    {
        var sql = "SELECT * FROM posts WHERE post_uid = @Id";
        var result = await _npgsqlConnection.QueryFirstOrDefaultAsync(sql, new { Id = id });
        if (result == null)
            return null!;

        var post = new Post
        {
            Id = (Guid)result.post_uid,
            UserId = (Guid)result.user_uid,
            Title = (string)result.title,
            Content = (string)result.content,
            CreatedAt = (DateTime)result.created_at
        };
        return post;
    }

    public async Task<Post> GetPostByTitle(string title)
    {
        var sql = "SELECT * FROM posts WHERE title = @Title";
        var result = await _npgsqlConnection.QueryFirstOrDefaultAsync(sql, new { Title = title }, transaction: _dbTransaction);
        if (result == null)
            return null!;

        var post = new Post
        {
            Id = (Guid)result.post_uid,
            UserId = (Guid)result.user_uid,
            Title = (string)result.title,
            Content = (string)result.content,
            CreatedAt = (DateTime)result.created_at
        };
        return post;
    }
    public async Task<IReadOnlyList<Post>> GetPostsByUser(Guid id)
    {
        var sql = """
        SELECT * FROM posts WHERE user_uid = @UserId
        INNER JOIN users ON posts.user_uid = users.user_uid
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
            WHERE post_uid = @Id
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql, post, transaction: _dbTransaction);
        return result;
    }
}
