using Microsoft.AspNetCore.Mvc;
using NotesApi.Common;
using NotesApi.DTOs;
using NotesApi.Services;

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

    // -- List User
    [HttpGet("list")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<UserResponseDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<UserResponseDto>>>> ListUsers()
    {
        var users = await _userService.GetAllUsers();
        return Ok(ApiResponse<IEnumerable<UserResponseDto>>.Success(users));
    }
}
