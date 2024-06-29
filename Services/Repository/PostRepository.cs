
using System.Data;
using BlogPlatformApi.Mapping;
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
            VALUES (uuid_generate_v4(), @user_uid, @title, @content, @created_at)
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql, post, transaction: _dbTransaction);
        return result;
    }

    public async Task<int> DeleteAsync(Guid id)
    {
        var sql = "DELETE FROM posts WHERE post_uid = @post_uid";
        var result = await _npgsqlConnection.ExecuteAsync(sql, new { post_uid = id }, transaction: _dbTransaction);
        return result;
    }

    public async Task<IReadOnlyList<Post>> GetAllAsync()
    {
        var sql = """
            SELECT * FROM posts 
            INNER JOIN users ON posts.user_uid = users.user_uid
        """;
        var result = await _npgsqlConnection.QueryAsync<Post, BlogUser, Post>(sql,
            (post, user) =>
            {
                post.user = user;
                return post;
            }, splitOn: "user_uid", transaction: _dbTransaction);

        return result.ToList();
    }

    public async Task<Post?> GetByIdAsync(Guid id)
    {
        var sql =
        """
            SELECT * FROM posts
            INNER JOIN users ON posts.user_uid = users.user_uid
            WHERE post_uid = @post_uid
        """;
        var result = await _npgsqlConnection.QueryAsync<Post, BlogUser, Post>(sql,
        (post, user) =>
        {
            post.user = user;
            return post;
        },

        new { post_uid = id },
        splitOn: "user_uid");
        var post = result.FirstOrDefault();

        if (post == null)
            return null!;

        return post;
    }

    public async Task<Post> GetPostByTitle(string title)
    {
        var sql =
        """
            SELECT * FROM posts
            INNER JOIN users ON posts.user_uid = users.user_uid
            WHERE title = @title
        """;
        var result = await _npgsqlConnection.QueryAsync<Post, BlogUser, Post>(sql,
        (post, user) =>
        {
            post.user = user;
            return post;
        },

        new { title },
        splitOn: "user_uid");
        var post = result.FirstOrDefault();

        if (post == null)
            return null!;

        return post;
    }

    public async Task<IReadOnlyList<Post>> GetPostsByUser(Guid id)
    {
        var sql = """
            SELECT * FROM posts 
            INNER JOIN users ON posts.user_uid = users.user_uid
            WHERE posts.user_uid = @user_uid
        """;
        var result = await _npgsqlConnection.QueryAsync<Post, BlogUser, Post>(sql,
            (post, user) =>
            {
                post.user = user;
                return post;
            }, new { user_uid = id }, splitOn: "user_uid", transaction: _dbTransaction);

        return result.ToList();
    }

    public async Task<int> UpdateAsync(Post post)
    {
        var sql =
        """
            UPDATE posts SET
            title = @title,
            content = @content
            WHERE post_uid = @post_uid
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql, new
        {
            post.title,
            post.content,
            post.post_uid
        }, transaction: _dbTransaction);
        return result;
    }
}
