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
            VALUES(uuid_generate_v4(), @TagName)
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql, tags, transaction: _dbTransaction);
        return result;
    }

    public async Task<int> DeleteAsync(Guid id)
    {
        var sql = "DELETE FROM tags WHERE tag_uid = @Id";
        var result = await _npgsqlConnection.ExecuteAsync(sql, new { Id = id }, transaction: _dbTransaction);
        return result;
    }

    public async Task<IReadOnlyList<Tags>> GetAllAsync()
    {
        var sql = "SELECT * FROM tags";
        var rawResult = await _npgsqlConnection.QueryAsync(sql);
        if (rawResult == null)
            return null!;

        var result = rawResult.Select(x => new Tags
        {
            Id = (Guid)x.tag_uid,
            TagName = (string)x.tag_name
        });
        return result.ToList();
    }

    public async Task<Tags?> GetByIdAsync(Guid id)
    {
        var sql = "SELECT * FROM tags WHERE tag_uid = @Id";
        var result = await _npgsqlConnection.QueryFirstOrDefaultAsync(sql, new { Id = id });
        if (result == null)
            return null!;

        var tag = new Tags
        {
            Id = (Guid)result.tag_uid,
            TagName = (string)result.tag_name
        };
        return tag;
    }

    public async Task<int> UpdateAsync(Tags tags)
    {
        var sql = """
            UPDATE tags SET
            tag_name = @TagName
            WHERE tag_uid = @Id
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql, tags, transaction: _dbTransaction);
        return result;
    }
}
