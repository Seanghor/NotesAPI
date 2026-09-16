using System.Security.Cryptography;
using System.Text;
using NotesApi.Services.Interfaces;

namespace NotesApi.Services;

public class PasswordHasher : IPasswordHasher
{
    // Hash with >>SHA-256
    public string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToHexString(bytes);
    }

    // Verify 
    public bool VerifyPassword(string password, string passwordHash)
    {
        string hashedInput = HashPassword(password);
        return string.Equals(hashedInput, passwordHash, StringComparison.OrdinalIgnoreCase);
    }
}
