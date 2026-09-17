using Dapper;
using NotesApi.Models;
using NotesApi.Repositories;
using NotesApi.Services;

namespace NotesApi.Data;

public static class Seed
{
    public static async Task SeedAdmin(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var noteRepository = scope.ServiceProvider.GetRequiredService<INoteRepository>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        // Seed User 1: 'techbodia'
        var user1 = await userRepository.GetByUsernameAsync("techbodia");
        if (user1 is null)
        {
            var defaultUser1 = new User
            {
                Username = "techbodia",
                PasswordHash = passwordHasher.HashPassword("admin@12345"),
                Role = "User",
                CreatedAt = DateTime.UtcNow
            };

            var user1Id = await userRepository.CreateAsync(defaultUser1);
            Console.WriteLine($"--> [Seed] User created: (Username: techbodia, Password: admin@12345, Id: {user1Id})");

            var techbodiaNotes = new[]
            {
                // Personal
                new Note { Title = "Morning Routine & Habits", Content = "1. Meditate 10 mins\n2. 30 mins workout before standup\n3. Read 15 pages of a technical book.", Category = "Personal", UserId = user1Id, CreatedAt = new DateTime(2026, 8, 12, 7, 30, 0, DateTimeKind.Utc) },
                new Note { Title = "Weekend Getaway Plan", Content = "Book flight tickets to Siem Reap and check hotel options near Angkor Wat.", Category = "Personal", UserId = user1Id, CreatedAt = new DateTime(2026, 9, 1, 19, 0, 0, DateTimeKind.Utc) },

                // Work
                new Note { Title = "Project Roadmap & Planning", Content = "Discussion on Q3 and Q4 milestones with engineering and product leads.", Category = "Work", UserId = user1Id, CreatedAt = new DateTime(2026, 9, 2, 9, 30, 0, DateTimeKind.Utc) },
                new Note { Title = "Meeting notes with Client", Content = "Client feedback on API response envelope format, pagination parameters, and authentication flow.", Category = "Work", UserId = user1Id, CreatedAt = new DateTime(2026, 9, 10, 14, 0, 0, DateTimeKind.Utc) },
                new Note { Title = "Sprint 14 Retrospective", Content = "Fast deployment pipeline achieved. Action item: Increase automated integration test coverage.", Category = "Work", UserId = user1Id, CreatedAt = new DateTime(2026, 9, 18, 16, 0, 0, DateTimeKind.Utc) },

                // Study
                new Note { Title = "ASP.NET Core Performance Tuning", Content = "Reviewing MemoryCache vs DistributedCache with Redis, Connection Pooling in Dapper, and JIT optimizations.", Category = "Study", UserId = user1Id, CreatedAt = new DateTime(2026, 8, 25, 20, 15, 0, DateTimeKind.Utc) },
                new Note { Title = "Database Indexing Best Practices", Content = "Clustered vs Non-Clustered index trade-offs, composite index column order, and execution plan analysis in MSSQL.", Category = "Study", UserId = user1Id, CreatedAt = new DateTime(2026, 9, 8, 11, 0, 0, DateTimeKind.Utc) },

                // Ideas
                new Note { Title = "Welcome to NotesApi", Content = "Getting started with NotesApi tutorial and brainstorming new features.", Category = "Ideas", UserId = user1Id, CreatedAt = new DateTime(2026, 8, 15, 8, 0, 0, DateTimeKind.Utc) },
                new Note { Title = "AI Note Summarization Feature", Content = "Explore integrating OpenAI/Gemini API to auto-tag and generate executive summaries for long notes.", Category = "Ideas", UserId = user1Id, CreatedAt = new DateTime(2026, 9, 14, 13, 20, 0, DateTimeKind.Utc) },

                // Todo
                new Note { Title = "Bug report & Fix notes", Content = "Fixed Dapper query parameter issue and verified null handling across search filters.", Category = "Todo", UserId = user1Id, CreatedAt = new DateTime(2026, 9, 20, 16, 45, 0, DateTimeKind.Utc) },
                new Note { Title = "Release 1.0 Checklist", Content = "Final security check, rate limiting, and JWT token validation test.", Category = "Todo", UserId = user1Id, CreatedAt = new DateTime(2026, 10, 1, 10, 0, 0, DateTimeKind.Utc) },
                new Note { Title = "Update Swagger Documentation", Content = "Sync OpenAPI models with latest DTOs and standard ApiResponse envelope formats.", Category = "Todo", UserId = user1Id, CreatedAt = new DateTime(2026, 10, 3, 15, 30, 0, DateTimeKind.Utc) }
            };

            foreach (var note in techbodiaNotes)
            {
                await noteRepository.CreateAsync(note);
            }
            Console.WriteLine($"--> [Seed] {techbodiaNotes.Length} notes created for 'techbodia'");
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
                CreatedAt = new DateTime(2026, 8, 10, 0, 0, 0, DateTimeKind.Utc)
            };

            var user2Id = await userRepository.CreateAsync(defaultUser2);
            Console.WriteLine($"--> [Seed] User created: (Username: seanghor, Password: admin@12345, Id: {user2Id})");

            var seanghorNotes = new[]
            {
                // Personal
                new Note { Title = "Seanghor's First Note", Content = "This is a private note for Seanghor! Welcome to the new workspace.", Category = "Personal", UserId = user2Id, CreatedAt = new DateTime(2026, 8, 20, 9, 0, 0, DateTimeKind.Utc) },
                new Note { Title = "Monthly Budget & Financial Goals", Content = "Track monthly savings, investment portfolio distribution, and tech gadget budget.", Category = "Personal", UserId = user2Id, CreatedAt = new DateTime(2026, 9, 3, 18, 45, 0, DateTimeKind.Utc) },
                new Note { Title = "Favorite Books & Reading List", Content = "1. Clean Architecture by Robert C. Martin\n2. Designing Data-Intensive Applications\n3. Atomic Habits by James Clear.", Category = "Personal", UserId = user2Id, CreatedAt = new DateTime(2026, 9, 12, 21, 0, 0, DateTimeKind.Utc) },

                // Work
                new Note { Title = "Docker setup and commands", Content = "Running MSSQL server in Docker container on port 1433 with custom volume mounting.", Category = "Work", UserId = user2Id, CreatedAt = new DateTime(2026, 9, 25, 18, 0, 0, DateTimeKind.Utc) },
                new Note { Title = "Production Deployment Checklist", Content = "Verify environment secrets in appsettings, setup SSL certificate, and configure health check endpoints.", Category = "Work", UserId = user2Id, CreatedAt = new DateTime(2026, 9, 28, 14, 10, 0, DateTimeKind.Utc) },
                new Note { Title = "Team Standup Notes", Content = "Completed JWT token refresh service, currently testing CORS configuration with frontend client.", Category = "Work", UserId = user2Id, CreatedAt = new DateTime(2026, 10, 2, 9, 0, 0, DateTimeKind.Utc) },

                // Study
                new Note { Title = "Learn C# and Dapper", Content = "Practicing CRUD operations, parameter binding, transactions, and multi-mapping with Dapper.", Category = "Study", UserId = user2Id, CreatedAt = new DateTime(2026, 9, 5, 11, 15, 0, DateTimeKind.Utc) },
                new Note { Title = "Backend API Architecture notes", Content = "3-tier architecture with Repository and Service layers, Dependency Injection lifecycles (Scoped vs Transient vs Singleton).", Category = "Study", UserId = user2Id, CreatedAt = new DateTime(2026, 9, 15, 15, 30, 0, DateTimeKind.Utc) },
                new Note { Title = "TypeScript and React State Management", Content = "Comparison of Zustand vs Redux Toolkit vs TanStack React Query for caching server state.", Category = "Study", UserId = user2Id, CreatedAt = new DateTime(2026, 9, 22, 16, 20, 0, DateTimeKind.Utc) },

                // Ideas
                new Note { Title = "Future learning goals", Content = "Microservices architecture, gRPC communication, Redis caching, and unit testing with xUnit.", Category = "Ideas", UserId = user2Id, CreatedAt = new DateTime(2026, 10, 5, 8, 0, 0, DateTimeKind.Utc) },
                new Note { Title = "Mobile App for Notes", Content = "Build a Flutter or React Native companion mobile app with offline-first synchronization using SQLite.", Category = "Ideas", UserId = user2Id, CreatedAt = new DateTime(2026, 10, 8, 12, 0, 0, DateTimeKind.Utc) },

                // Todo
                new Note { Title = "Refactor Authentication Middleware", Content = "Implement custom unauthorized response payload matching ApiResponse envelope format.", Category = "Todo", UserId = user2Id, CreatedAt = new DateTime(2026, 9, 18, 10, 30, 0, DateTimeKind.Utc) },
                new Note { Title = "Implement Full-text Search", Content = "Add search query filter across note titles and content with pagination support.", Category = "Todo", UserId = user2Id, CreatedAt = new DateTime(2026, 9, 30, 17, 0, 0, DateTimeKind.Utc) },
                new Note { Title = "Setup CI/CD GitHub Actions", Content = "Automate build, test, and docker image build on push to main branch.", Category = "Todo", UserId = user2Id, CreatedAt = new DateTime(2026, 10, 6, 11, 45, 0, DateTimeKind.Utc) }
            };

            foreach (var note in seanghorNotes)
            {
                await noteRepository.CreateAsync(note);
            }
            Console.WriteLine($"--> [Seed] {seanghorNotes.Length} notes created for 'seanghor'");
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
