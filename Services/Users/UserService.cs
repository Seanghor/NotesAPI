using NotesApi.DTOs;
using NotesApi.Models;
using NotesApi.Repositories;

namespace NotesApi.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    // -- service:--> Get All
    public async Task<IEnumerable<UserResponseDto>> GetAllUsers()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(u => new UserResponseDto(
            u.Id, 
            u.Username, 
            u.Role, 
            u.CreatedAt
        ));
    }

    // -- service:--> Get By ID
    public async Task<UserResponseDto?> GetUserById(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user is null) return null;

        return new UserResponseDto(
            user.Id, 
            user.Username, 
            user.Role, 
            user.CreatedAt
        );
    }

    // -- service:--> Get By Username
    public async Task<User?> GetUserByUsername(string username)
    {
        return await _userRepository.GetByUsernameAsync(username);
    }

    // -- service:--> Create 
    public async Task<UserResponseDto> CreateUser(CreateUserDto dto)
    {
        var exists = await _userRepository.ExistsByUsernameAsync(dto.Username);
        if (exists)
        {
            throw new InvalidOperationException($"Username '{dto.Username}' is already taken.");
        }

        var user = new User
        {
            Username = dto.Username.Trim(),
            PasswordHash = _passwordHasher.HashPassword(dto.Password),
            Role = string.IsNullOrWhiteSpace(dto.Role) ? "User" : dto.Role.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        var newId = await _userRepository.CreateAsync(user);
        user.Id = newId;

        return new UserResponseDto(
            user.Id, 
            user.Username, 
            user.Role, 
            user.CreatedAt
        );
    }
}
