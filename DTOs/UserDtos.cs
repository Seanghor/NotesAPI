using System.ComponentModel.DataAnnotations;

namespace NotesApi.DTOs;

// Create Dto
public record CreateUserDto(
    [Required(ErrorMessage = "Username is mandatory.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters.")]
    string Username,

    [Required(ErrorMessage = "Password is mandatory.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
    string Password,

    string Role = "User"
);

// Response Dto
public record UserResponseDto(
    int Id,
    string Username,
    string Role,
    DateTime CreatedAt
);
