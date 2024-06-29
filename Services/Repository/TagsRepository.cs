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
            tag_name = @tag_name
            WHERE tag_uid = @tag_uid
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql, tags, transaction: _dbTransaction);
        return result;
    }
}
