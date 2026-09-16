using System.ComponentModel.DataAnnotations;

namespace NotesApi.DTOs;

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
