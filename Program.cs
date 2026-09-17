using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NotesApi.Data;
using NotesApi.Repositories;
using NotesApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Controller & API Services
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();

// JWT Authentication & Authorization
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["Key"] 
    ?? throw new InvalidOperationException("JWT Secret Key 'Jwt:Key' is not configured.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.MapInboundClaims = false; // Keeps clean claim names (id, username, role)
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"] ?? "NotesApi",
        ValidAudience = jwtSettings["Audience"] ?? "NotesApiUsers",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

builder.Services.AddAuthorization();

// Dependency Injection
builder.Services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.Services.AddSingleton<ITokenService, TokenService>();

builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<INoteService, NoteService>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

//  Commands (--reset, --seed) or Startup Auto-Seed
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
