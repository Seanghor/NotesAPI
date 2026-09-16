using Dapper;
using NotesApi.Models;
using NotesApi.Repositories.Interfaces;
using NotesApi.Services.Interfaces;

namespace NotesApi.Data;

public static class Seed
{
    public static async Task SeedAdmin(IServiceProvider serviceProvider){
        using var scope = serviceProvider.CreateScope();
        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var noteRepository = scope.ServiceProvider.GetRequiredService<INoteRepository>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        // seed user1
        var user1 = await userRepository.GetByUsernameAsync("techbodia");
        if (user1 is null)
        {
            var defaultUser1 = new User
            {
                Username = "techbodia",
                PasswordHash = passwordHasher.HashPassword("admin@12345"),
                Role = "User",
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            var user1Id = await userRepository.CreateAsync(defaultUser1);
            Console.WriteLine($"--> [Seed] User created: (Username: techbodia, Password: admin@12345, Id: {user1Id})");

            var techbodiaNotes = new[]
            {
                new Note { Title = "Welcome to NotesApi", Content = "Getting started with NotesApi tutorial.", UserId = user1Id, CreatedAt = new DateTime(2026, 8, 15, 8, 0, 0, DateTimeKind.Utc) },
                new Note { Title = "Project Roadmap & Planning", Content = "Discussion on Q3 and Q4 milestones.", UserId = user1Id, CreatedAt = new DateTime(2026, 9, 2, 9, 30, 0, DateTimeKind.Utc) },
                new Note { Title = "Meeting notes with Client", Content = "Client feedback on API response envelope format.", UserId = user1Id, CreatedAt = new DateTime(2026, 9, 10, 14, 0, 0, DateTimeKind.Utc) },
                new Note { Title = "Bug report & Fix notes", Content = "Fixed Dapper query parameter issue.", UserId = user1Id, CreatedAt = new DateTime(2026, 9, 20, 16, 45, 0, DateTimeKind.Utc) },
                new Note { Title = "Release 1.0 Checklist", Content = "Final security check and token validation test.", UserId = user1Id, CreatedAt = new DateTime(2026, 10, 1, 10, 0, 0, DateTimeKind.Utc) }
            };

            foreach (var note in techbodiaNotes)
            {
                await noteRepository.CreateAsync(note);
            }
            Console.WriteLine($"--> [Seed] 5 notes created for 'techbodia'");
        }

        // Seed User 2: 'seanghor'
        var user2 = await userRepository.GetByUsernameAsync("seanghor");
        if (user2 is null)
        {
            var defaultUser2 = new User
            {
                Username = "seanghor",
                PasswordHash = passwordHasher.HashPassword("admin@12345"),
                Role = "User",
                CreatedAt = new DateTime(2026, 8, 10, 0, 0, 0, DateTimeKind.Utc),
                IsDeleted = false
            };

            var user2Id = await userRepository.CreateAsync(defaultUser2);
            Console.WriteLine($"--> [Seed] User created: (Username: seanghor, Password: admin@12345, Id: {user2Id})");

            var seanghorNotes = new[]
            {
                new Note { Title = "Seanghor's First Note", Content = "This is a private note for Seanghor!", UserId = user2Id, CreatedAt = new DateTime(2026, 8, 20, 9, 0, 0, DateTimeKind.Utc) },
                new Note { Title = "Learn C# and Dapper", Content = "Practicing CRUD operations and clean architecture.", UserId = user2Id, CreatedAt = new DateTime(2026, 9, 5, 11, 15, 0, DateTimeKind.Utc) },
                new Note { Title = "Backend API Architecture notes", Content = "3-tier architecture with Repository and Service layers.", UserId = user2Id, CreatedAt = new DateTime(2026, 9, 15, 15, 30, 0, DateTimeKind.Utc) },
                new Note { Title = "Docker setup and commands", Content = "Running MSSQL server in Docker container on port 1433.", UserId = user2Id, CreatedAt = new DateTime(2026, 9, 25, 18, 0, 0, DateTimeKind.Utc) },
                new Note { Title = "Future learning goals", Content = "Microservices, Redis caching, and unit testing.", UserId = user2Id, CreatedAt = new DateTime(2026, 10, 5, 8, 0, 0, DateTimeKind.Utc) }
            };

            foreach (var note in seanghorNotes)
            {
                await noteRepository.CreateAsync(note);
            }
            Console.WriteLine($"--> [Seed] 5 notes created for 'seanghor'");
        }
    }




    public static async Task ResetDatabase(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var connectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();

        using var connection = connectionFactory.CreateConnection();

        Console.WriteLine("--> [Reset] database tables...");

        const string sql = """
            TRUNCATE TABLE Notes;
            DELETE FROM Users;
            DBCC CHECKIDENT ('Users', RESEED, 0);
            """;

        await connection.ExecuteAsync(sql);
        Console.WriteLine("--> [Reset] All tables");

        // Re-seed 
        await SeedAdmin(serviceProvider);
    }
}
