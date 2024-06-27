
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
        var sql = """
            SELECT * FROM posts 
            INNER JOIN users ON posts.user_uid = users.user_uid
        """;
        var rawResult = await _npgsqlConnection.QueryAsync<PostMapp, BlogUserMapp, PostMapp>(sql,
            (post, user) =>
            {
                post.User = user;
                return post;
            }, splitOn: "user_uid", transaction: _dbTransaction);
        var result = rawResult.Select(x => new Post
        {
            Id = x.post_uid,
            UserId = x.user_uid,
            User = x.User,
            Title = x.Title,
            Content = x.Content,
            CreatedAt = x.created_at
        });
        return result.ToList();
    }

    public async Task<Post?> GetByIdAsync(Guid id)
    {
        var sql =
        """
            SELECT * FROM posts
            INNER JOIN users ON posts.user_uid = users.user_uid
            WHERE post_uid = @Id
        """;
        var result = await _npgsqlConnection.QueryAsync<PostMapp, BlogUserMapp, PostMapp>(sql,
        (post, user) =>
        {
            post.User = user;
            return post;
        },

        new { Id = id },
        splitOn: "user_uid");
        var postMap = result.FirstOrDefault();

        if (postMap == null)
            return null!;

        var post = new Post
        {
            Id = postMap.post_uid,
            UserId = postMap.user_uid,
            User = postMap.User,
            Title = postMap.Title,
            Content = postMap.Content,
            CreatedAt = postMap.created_at
        };

        return post;
    }

    public async Task<Post> GetPostByTitle(string title)
    {
        var sql =
        """
            SELECT * FROM posts
            INNER JOIN users ON posts.user_uid = users.user_uid
            WHERE title = @Title
        """;
        var result = await _npgsqlConnection.QueryAsync<PostMapp, BlogUserMapp, PostMapp>(sql,
        (post, user) =>
        {
            post.User = user;
            return post;
        },

        new { Title = title },
        splitOn: "user_uid");
        var postMap = result.FirstOrDefault();

        if (postMap == null)
            return null!;

        var post = new Post
        {
            Id = postMap.post_uid,
            UserId = postMap.user_uid,
            User = postMap.User,
            Title = postMap.Title,
            Content = postMap.Content,
            CreatedAt = postMap.created_at
        };

        return post;
    }

    public async Task<IReadOnlyList<Post>> GetPostsByUser(Guid id)
    {
        var sql = """
            SELECT * FROM posts 
            INNER JOIN users ON posts.user_uid = users.user_uid
            WHERE posts.user_uid = @UserId
        """;
        var rawResult = await _npgsqlConnection.QueryAsync<PostMapp, BlogUserMapp, PostMapp>(sql,
            (post, user) =>
            {
                post.User = user;
                return post;
            }, new { UserId = id }, splitOn: "user_uid", transaction: _dbTransaction);
        var result = rawResult.Select(x => new Post
        {
            Id = x.post_uid,
            UserId = x.user_uid,
            User = x.User,
            Title = x.Title,
            Content = x.Content,
            CreatedAt = x.created_at
        });


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
