using System.ComponentModel.DataAnnotations;

namespace NotesApi.DTOs;

// -- Register DTO
public record RegisterDto(
    [Required(ErrorMessage = "Username is required.")]
    [StringLength(15, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 15 characters.")]
    string Username,

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
    string Password,

    string Role = "User"
);

// -- Login DTO
public record LoginDto(
    [Required(ErrorMessage = "Username is required.")]
    string Username,

    [Required(ErrorMessage = "Password is required.")]
    string Password
);



// -- Login Response DTO
public record AuthResponseDto(
    string Token,
    int UserId,
    string Username,
    string Role,
    DateTime ExpiresAt
);
