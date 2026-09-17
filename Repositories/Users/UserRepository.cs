using Dapper;
using NotesApi.Data;
using NotesApi.Models;

namespace NotesApi.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    public UserRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sqlQuery = """
            SELECT Id, Username, PasswordHash, Role, CreatedAt
            FROM Users
            ORDER BY CreatedAt DESC;
            """;

        return await connection.QueryAsync<User>(sqlQuery);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sqlQuery = """
            SELECT Id, Username, PasswordHash, Role, CreatedAt
            FROM Users
            WHERE Id = @Id;
            """;

        var paramaters = new { Id = id };
        return await connection.QuerySingleOrDefaultAsync<User>(sqlQuery, paramaters);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sqlQuery = """
            SELECT Id, Username, PasswordHash, Role, CreatedAt
            FROM Users
            WHERE Username = @Username;
            """;

        var paramaters = new { Username = username };
        return await connection.QuerySingleOrDefaultAsync<User>(sqlQuery, paramaters);
    }

    public async Task<bool> ExistsByUsernameAsync(string username)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sqlQuery = """
            SELECT COUNT(1)
            FROM Users
            WHERE Username = @Username;
            """;

        var paramaters = new { Username = username };
        var count = await connection.ExecuteScalarAsync<int>(sqlQuery, paramaters);
        return count > 0;
    }

    public async Task<int> CreateAsync(User user)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sqlQuery = """
            INSERT INTO Users (Username, PasswordHash, Role, CreatedAt)
            OUTPUT INSERTED.Id
            VALUES (@Username, @PasswordHash, @Role, @CreatedAt);
            """;

        return await connection.ExecuteScalarAsync<int>(sqlQuery, user);
    }
}
