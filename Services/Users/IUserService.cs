using NotesApi.DTOs;
using NotesApi.Models;

namespace NotesApi.Services;

public interface IUserService
{
    Task<IEnumerable<UserResponseDto>> GetAllUsers();
    Task<UserResponseDto?> GetUserById(int id);
    Task<UserResponseDto> CreateUser(CreateUserDto dto);
    Task<User?> GetUserByUsername(string username);
}
