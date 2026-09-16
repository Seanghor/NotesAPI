using NotesApi.DTOs;
using NotesApi.Models;

namespace NotesApi.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
    Task<UserResponseDto?> GetUserByIdAsync(int id);
    Task<UserResponseDto> CreateUserAsync(CreateUserDto dto);
    Task<User?> GetUserByUsernameAsync(string username);
}
