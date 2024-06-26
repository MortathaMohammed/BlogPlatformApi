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
        INSERT INTO tags (tag_id, tag_name)
        VALUES(@Id, @TagName)
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql, tags, transaction: _dbTransaction);
        return result;
    }

    public async Task<int> DeleteAsync(string id)
    {
        var sql = "DELETE FROM tags WHERE tag_id = @Id";
        var result = await _npgsqlConnection.ExecuteAsync(sql, new { Id = id });
        return result;
    }

    public async Task<IReadOnlyList<Tags>> GetAllAsync()
    {
        var sql = "SELECT * FROM tags";
        var result = await _npgsqlConnection.QueryAsync<Tags>(sql, transaction: _dbTransaction);
        return result.ToList();
    }

    public async Task<Tags?> GetByIdAsync(string id)
    {
        var sql = "SELECT * FROM tags WHERE tag_id = @Id";
        var result = await _npgsqlConnection.QueryFirstOrDefaultAsync<Tags>(sql, new { Id = id }, transaction: _dbTransaction);
        return result;
    }

    public async Task<int> UpdateAsync(Tags tags)
    {
        var sql = """
        UPDATE tags SET
        tag_name = @TagName
        WHERE tag_id = @Id
        """;
        var result = await _npgsqlConnection.ExecuteAsync(sql, tags, transaction: _dbTransaction);
        return result;
    }
}
