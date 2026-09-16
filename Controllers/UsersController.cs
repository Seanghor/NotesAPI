using Microsoft.AspNetCore.Mvc;
using NotesApi.Common;
using NotesApi.DTOs;
using NotesApi.Services.Interfaces;

namespace NotesApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    // -- Create ..
    [HttpPost("create")]
    [ProducesResponseType(typeof(ApiResponse<UserResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<UserResponseDto>>> CreateUser([FromBody] CreateUserDto userDto)
    {
        try
        {
            var createdUser = await _userService.CreateUserAsync(userDto);
            return Ok(ApiResponse<UserResponseDto>.Success(createdUser, "User created successfully", StatusCodes.Status200OK));
        }
        catch (InvalidOperationException error)
        {
            return BadRequest(ApiResponse.Error(error.Message, StatusCodes.Status400BadRequest));
        }
    }

    // -- List User
    [HttpGet("list")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<UserResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<UserResponseDto>>>> ListUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(ApiResponse<IEnumerable<UserResponseDto>>.Success(users));
    }
}
