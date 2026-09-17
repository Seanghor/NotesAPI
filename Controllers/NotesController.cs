using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesApi.Common;
using NotesApi.DTOs;
using NotesApi.Services;

namespace NotesApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NotesController : ControllerBase
{
    private readonly INoteService _noteService;

    public NotesController(INoteService noteService)
    {
        _noteService = noteService;
    }

    // -- GET ALL
    [HttpGet("list")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<NoteDetailDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<IEnumerable<NoteDetailDto>>>> GetAll([FromQuery] NoteQueryDto queryParams)
    {
        var notes = await _noteService.GetNotesForUser(User.GetUserId(), queryParams);
        return Ok(ApiResponse<IEnumerable<NoteDetailDto>>.Success(notes, "Get list notes successfully", StatusCodes.Status200OK));
    }

    // -- GET BY ID
    [HttpGet("getByID/{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<NoteDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<NoteDetailDto>>> GetById(int id)
    {
        var note = await _noteService.GetNoteByIdForUser(id, User.GetUserId());
        if (note is null)
        {
            return NotFound(ApiResponse.Error($"Note with ID {id} not found.", StatusCodes.Status404NotFound));
        }

        return Ok(ApiResponse<NoteDetailDto>.Success(note, "Note retrieved successfully", StatusCodes.Status200OK));
    }

    // CREATE
    [HttpPost("create")]
    [ProducesResponseType(typeof(ApiResponse<NoteDetailDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<NoteDetailDto>>> Create([FromBody] CreateNoteDto userDto)
    {
        var createdNote = await _noteService.CreateNote(userDto, User.GetUserId());

        return StatusCode(
            StatusCodes.Status201Created,
            ApiResponse<NoteDetailDto>.Success(createdNote, "Note created successfully", StatusCodes.Status201Created)
        );
    }

    // -- UPDATE BY ID
    [HttpPut("updateByID/{id:int}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse>> Update(int id, [FromBody] UpdateNoteDto dto)
    {
        var updated = await _noteService.UpdateNote(id, dto, User.GetUserId());
        if (!updated)
        {
            return NotFound(ApiResponse.Error($"Note with ID {id} not found.", StatusCodes.Status404NotFound));
        }

        return Ok(ApiResponse.Success("Note updated successfully", StatusCodes.Status200OK));
    }

    // -- DELETE BY ID
    [HttpDelete("deleteByID/{id:int}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse>> Delete(int id)
    {
        var deleted = await _noteService.DeleteNote(id, User.GetUserId());
        if (!deleted)
        {
            return NotFound(ApiResponse.Error($"Note with ID {id} not found.", StatusCodes.Status404NotFound));
        }

        return Ok(ApiResponse.Success("Note deleted successfully", StatusCodes.Status200OK));
    }
}
