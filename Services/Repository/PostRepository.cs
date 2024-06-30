
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

    public async Task<int> AddPostTagId(PostTags postTags)
    {
        var sql =
        """
            INSERT INTO post_tags(post_uid, tag_uid)
            VALUES (@PostId, @TagId)
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql, new { PostId = postTags.post_uid, TagId = postTags.tag_uid }, transaction: _dbTransaction);
        return result;
    }

    public async Task<int> DeleteAsync(Guid id)
    {
        var sql = "DELETE FROM posts WHERE post_uid = @post_uid";
        var result = await _npgsqlConnection.ExecuteAsync(sql, new { post_uid = id }, transaction: _dbTransaction);
        return result;
    }

    public async Task<int> DeletePostTagId(Guid postId, Guid tagId)
    {
        var sql =
        """
            DELETE FROM post_tags WHERE post_tags.post_uid = @PostId AND post_tags.tag_uid = @TagId
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql,
        new { PostId = postId, TagId = tagId },
        transaction: _dbTransaction);
        return result;
    }

    public async Task<IReadOnlyList<Post>> GetAllAsync()
    {
        var sql = """
            SELECT * FROM posts 
            INNER JOIN users ON posts.user_uid = users.user_uid
            INNER JOIN post_tags ON posts.post_uid = post_tags.post_uid
            INNER JOIN tags ON post_tags.tag_uid = tags.tag_uid
        """;
        var result = await _npgsqlConnection.QueryAsync<Post, BlogUser, Tags, Post>(sql,
            (post, user, tags) =>
            {
                post.user = user;
                post.tags.Add(tags);
                return post;
            }, splitOn: "user_uid, post_uid", transaction: _dbTransaction);

        return result.ToList();
    }

    public async Task<Post?> GetByIdAsync(Guid id)
    {
        var sql =
        """
            SELECT * FROM posts
            INNER JOIN users ON posts.user_uid = users.user_uid
            INNER JOIN post_tags ON posts.post_uid = post_tags.post_uid
            INNER JOIN tags ON post_tags.tag_uid = tags.tag_uid
            WHERE posts.post_uid = @PostId
        """;
        var result = await _npgsqlConnection.QueryAsync<Post, BlogUser, Tags, Post>(sql,
        (post, user, tags) =>
        {
            post.user = user;
            post.tags.Add(tags);
            return post;
        },

        new { PostId = id },
        splitOn: "user_uid, post_uid");
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
            INNER JOIN post_tags ON posts.post_uid = post_tags.post_uid
            INNER JOIN tags ON post_tags.tag_uid = tags.tag_uid
            WHERE title = @Title
        """;
        var result = await _npgsqlConnection.QueryAsync<Post, BlogUser, Tags, Post>(sql,
        (post, user, tags) =>
        {
            post.user = user;
            post.tags.Add(tags);
            return post;
        },

        new { Title = title },
        splitOn: "user_uid, post_uid");
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
            INNER JOIN post_tags ON posts.post_uid = post_tags.post_uid
            INNER JOIN tags ON post_tags.tag_uid = tags.tag_uid
            WHERE posts.user_uid = @UserId
        """;
        var result = await _npgsqlConnection.QueryAsync<Post, BlogUser, Tags, Post>(sql,
            (post, user, tags) =>
            {
                post.user = user;
                post.tags.Add(tags);
                return post;
            }, new { UserId = id }, splitOn: "user_uid, post_uid", transaction: _dbTransaction);

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
