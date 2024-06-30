using System.Data;
using BlogPlatformApi.Models;
using BlogPlatformApi.Services.Repository.IRepository;
using Dapper;
using Npgsql;

namespace BlogPlatformApi.Services.Repository;
public class TagsRepository : ITagsRepository
{
    private readonly IDbTransaction _dbTransaction;
    private readonly NpgsqlConnection _npgsqlConnection;

    public TagsRepository(IDbTransaction dbTransaction, NpgsqlConnection npgsqlConnection)
    {
        _dbTransaction = dbTransaction;
        _npgsqlConnection = npgsqlConnection;
    }

    public async Task<int> AddAsync(Tags tags)
    {
        var sql = """
            INSERT INTO tags (tag_uid, tag_name)
            VALUES(uuid_generate_v4(), @tag_name)
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql, tags, transaction: _dbTransaction);
        return result;
    }

    public async Task<int> DeleteAsync(Guid id)
    {
        var sql = "DELETE FROM tags WHERE tag_uid = @tag_uid";
        var result = await _npgsqlConnection.ExecuteAsync(sql, new { tag_uid = id }, transaction: _dbTransaction);
        return result;
    }

    public async Task<IReadOnlyList<Tags>> GetAllAsync()
    {
        var sql = "SELECT * FROM tags";
        var result = await _npgsqlConnection.QueryAsync<Tags>(sql);

        if (result == null)
            return null!;

        return result.ToList();
    }

    public async Task<IReadOnlyList<Tags>> GetTagsWithPostsByPostId(Guid id)
    {
        var sql =
        """
            SELECT * FROM tags
            INNER JOIN post_tags ON tags.tag_uid = post_tags.tag_uid
            INNER JOIN posts ON post_tags.post_uid = posts.post_uid
            INNER JOIN users ON posts.user_uid = users.user_uid
            WHERE post_tags.post_uid = @PostId
        """;

        var result = await _npgsqlConnection.QueryAsync<Tags, Post, BlogUser, Tags>(sql,
        (tags, post, user) =>
        {
            post.user = user;
            tags.posts.Add(post);
            return tags;
        },
        new { PostId = id },
        splitOn: "post_uid, user_uid",
        transaction: _dbTransaction);
        if (result == null)
            return null!;
        return result.ToList();
    }

    public async Task<IReadOnlyList<Tags>> GetPostsByTagId(Guid id)
    {
        var sql =
        """
            SELECT * FROM tags
            INNER JOIN post_tags ON tags.tag_uid = post_tags.tag_uid
            INNER JOIN posts ON post_tags.post_uid = posts.post_uid
            INNER JOIN users ON posts.user_uid = users.user_uid
            WHERE post_tags.tag_uid = @TagId
        """;

        var result = await _npgsqlConnection.QueryAsync<Tags, Post, BlogUser, Tags>(sql,
        (tags, post, user) =>
        {
            post.user = user;
            tags.posts.Add(post);
            return tags;
        },
        new { TagId = id },
        splitOn: "post_uid, user_uid",
        transaction: _dbTransaction);
        if (result == null)
            return null!;
        return result.ToList();
    }

    public async Task<Tags?> GetByIdAsync(Guid id)
    {
        var sql = "SELECT * FROM tags WHERE tag_uid = @tag_uid";
        var result = await _npgsqlConnection.QueryFirstOrDefaultAsync<Tags>(sql, new { tag_uid = id });

        if (result == null)
            return null!;

        return result;
    }

    public async Task<int> UpdateAsync(Tags tags)
    {
        var sql = """
            UPDATE tags SET
            tag_name = @TagName
            WHERE tag_uid = @TagId
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql, new { TagName = tags.tag_name, TagId = tags.tag_uid }, transaction: _dbTransaction);
        return result;
    }

    public async Task<Tags> GetTagByName(string name)
    {
        var sql = "SELECT * FROM tags WHERE tag_name = @TagName";
        var result = await _npgsqlConnection.QueryFirstOrDefaultAsync<Tags>(sql, new { TagName = name });
        if (result == null)
            return null!;
        return result;
    }
}
