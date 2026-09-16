using Microsoft.AspNetCore.Mvc;
using NotesApi.Common;
using NotesApi.DTOs;
using NotesApi.Repositories.Interfaces;
using NotesApi.Services.Interfaces;

namespace NotesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public AuthController(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenService tokenService){
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    // -- Login
    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login(LoginDto loginDto){
        var user = await _userRepository.GetByUsernameAsync(loginDto.Username);
        if (user is null){
            return Unauthorized(ApiResponse.Error("Invalid username.", StatusCodes.Status401Unauthorized));
        }

        var isPasswordValid = _passwordHasher.VerifyPassword(loginDto.Password, user.PasswordHash);
        if (!isPasswordValid){
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
