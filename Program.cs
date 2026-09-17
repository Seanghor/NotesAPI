using NotesApi.Common;
using NotesApi.Data;
using NotesApi.Repositories;
using NotesApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();

// JWT Authentication & Authorization
builder.Services.AddJwtAuthentication(builder.Configuration);

// Dependency Injection
builder.Services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.Services.AddSingleton<ITokenService, TokenService>();

builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

//  Seed & Reset
if (args.Contains("--reset"))
{
    await Seed.ResetDatabase(app.Services);
    Console.WriteLine("--> Database has been reset and seeded successfully.");
    return;
}

if (args.Contains("--seed"))
{
    await Seed.SeedAdmin(app.Services);
    Console.WriteLine("--> Database has been seeded successfully.");
    return;
}

// Auto-seed default user & note on normal startup
await Seed.SeedAdmin(app.Services);

// HTTP Pipeline Configuration
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
Console.WriteLine("🚀 Server is running on: http://localhost:3000");

app.Run();
