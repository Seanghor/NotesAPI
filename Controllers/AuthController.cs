using Microsoft.AspNetCore.Mvc;
using NotesApi.Common;
using NotesApi.DTOs;
using NotesApi.Models;
using NotesApi.Repositories;
using NotesApi.Services;

namespace NotesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public AuthController(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    // -- Register
    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Register([FromBody] RegisterDto registerDto)
    {
        var existingUser = await _userRepository.ExistsByUsernameAsync(registerDto.Username.Trim());
        if (existingUser)
        {
            return BadRequest(ApiResponse.Error($"Username '{registerDto.Username.Trim()}' is already taken.", StatusCodes.Status400BadRequest));
        }

        var user = new User
        {
            Username = registerDto.Username.Trim(),
            PasswordHash = _passwordHasher.HashPassword(registerDto.Password),
            Role = "User",
            CreatedAt = DateTime.UtcNow
        };

        var newUserId = await _userRepository.CreateAsync(user);
        user.Id = newUserId;

        var (token, expiresAt) = _tokenService.GenerateToken(user);

        var authData = new AuthResponseDto(
            Token: token,
            UserId: user.Id,
            Username: user.Username,
            Role: user.Role,
            ExpiresAt: expiresAt
        );

        return StatusCode(
            StatusCodes.Status201Created,
            ApiResponse<AuthResponseDto>.Success(authData, "User registered successfully", StatusCodes.Status201Created)
        );
    }

    // -- Login
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login([FromBody] LoginDto loginDto)
    {
        var user = await _userRepository.GetByUsernameAsync(loginDto.Username);
        if (user is null)
        {
            return Unauthorized(ApiResponse.Error("Invalid username.", StatusCodes.Status401Unauthorized));
        }

        var isPasswordValid = _passwordHasher.VerifyPassword(loginDto.Password, user.PasswordHash);
        if (!isPasswordValid)
        {
            return Unauthorized(ApiResponse.Error("Incorrect password.", StatusCodes.Status401Unauthorized));
        }

        var (token, expiresAt) = _tokenService.GenerateToken(user);

        var authData = new AuthResponseDto(
            Token: token,
            UserId: user.Id,
            Username: user.Username,
            Role: user.Role,
            ExpiresAt: expiresAt
        );

        return Ok(ApiResponse<AuthResponseDto>.Success(authData, "Login successful", StatusCodes.Status200OK));
    }
}
