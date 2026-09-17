using Dapper;
using NotesApi.Data;
using NotesApi.Models;

namespace NotesApi.Repositories;

public class NoteRepository : INoteRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public NoteRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }


    public async Task<IEnumerable<Note>> GetAllByUserAsync(int userId, string? search, string? category, string? sortOrder)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sqlQuery = """
            SELECT Id, Title, Content, Category, UserId, CreatedAt, UpdatedAt
            FROM Notes
            WHERE UserId = @UserId
                AND (@Search IS NULL OR Title LIKE '%' + @Search + '%' OR Content LIKE '%' + @Search + '%')
                AND (@Category IS NULL OR @Category = 'All' OR Category = @Category)
            ORDER BY
                CASE WHEN @SortOrder = 'asc' THEN CreatedAt END ASC,
                CASE WHEN @SortOrder <> 'asc' OR @SortOrder IS NULL THEN CreatedAt END DESC;
            """;
        var paramaters = new  { 
            UserId = userId,
            Search = string.IsNullOrWhiteSpace(search) ? null : search.Trim(),
            Category = string.IsNullOrWhiteSpace(category) ? null : category.Trim(),
            SortOrder = sortOrder?.ToLowerInvariant()
        };
        return await connection.QueryAsync<Note>(sqlQuery, paramaters);
    }


    public async Task<Note?> GetByIdAndUserAsync(int id, int userId)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sqlQuery = """
            SELECT Id, Title, Content, Category, UserId, CreatedAt, UpdatedAt
            FROM Notes
            WHERE Id = @Id AND UserId = @UserId;
            """;

        var paramaters = new { Id = id, UserId = userId };
        return await connection.QuerySingleOrDefaultAsync<Note>(sqlQuery, paramaters);
    }

    public async Task<int> CreateAsync(Note note)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sqlQuery = """
            INSERT INTO Notes (Title, Content, Category, UserId, CreatedAt, UpdatedAt)
            OUTPUT INSERTED.Id
            VALUES (@Title, @Content, @Category, @UserId, @CreatedAt, @UpdatedAt);
            """;

        return await connection.ExecuteScalarAsync<int>(sqlQuery, note);
    }

    public async Task<bool> UpdateAsync(Note note)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sqlQuery = """
            UPDATE Notes
            SET Title = @Title,
                Content = @Content,
                Category = @Category,
                UpdatedAt = @UpdatedAt
            WHERE Id = @Id AND UserId = @UserId;
            """;

        var rowUpdated = await connection.ExecuteAsync(sqlQuery, note);
        return rowUpdated > 0;
    }


    public async Task<bool> DeleteAsync(int id, int userId)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sqlQuery = """
            DELETE FROM Notes
            WHERE Id = @Id AND UserId = @UserId;
            """;

        var paramaters = new { Id = id, UserId = userId };
        var rowDeleted = await connection.ExecuteAsync(sqlQuery, paramaters);
        return rowDeleted > 0;
    }
}
