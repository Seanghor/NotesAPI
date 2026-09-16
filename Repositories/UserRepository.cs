using Dapper;
using NotesApi.Data;
using NotesApi.Models;
using NotesApi.Repositories.Interfaces;

namespace NotesApi.Repositories;



public class UserRepository : IUserRepository{
    private readonly ISqlConnectionFactory _connectionFactory;

    public UserRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sqlQuery = """
            SELECT Id, Username, PasswordHash, Role, CreatedAt, IsDeleted
            FROM Users
            WHERE IsDeleted = 0
            ORDER BY CreatedAt DESC;
            """;

        return await connection.QueryAsync<User>(sqlQuery);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sqlQuery = """
            SELECT Id, Username, PasswordHash, Role, CreatedAt, IsDeleted
            FROM Users
            WHERE Id = @Id AND IsDeleted = 0;
            """;

        var paramaters = new { Id = id };
        return await connection.QuerySingleOrDefaultAsync<User>(sqlQuery, paramaters);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sqlQuery = """
            SELECT Id, Username, PasswordHash, Role, CreatedAt, IsDeleted
            FROM Users
            WHERE Username = @Username AND IsDeleted = 0;
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
            WHERE Username = @Username AND IsDeleted = 0;
            """;

        var paramaters = new { Username = username };
        var count = await connection.ExecuteScalarAsync<int>(sqlQuery, paramaters);
        return count > 0;
    }

    public async Task<int> CreateAsync(User user)
    {
        using var connection = _connectionFactory.CreateConnection();
        const string sqlQuery = """
            INSERT INTO Users (Username, PasswordHash, Role, CreatedAt, IsDeleted)
            OUTPUT INSERTED.Id
            VALUES (@Username, @PasswordHash, @Role, @CreatedAt, @IsDeleted);
            """;

        return await connection.ExecuteScalarAsync<int>(sqlQuery, user);
    }
}
