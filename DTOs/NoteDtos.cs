using System.ComponentModel.DataAnnotations;

namespace NotesApi.DTOs;

// -- Create DTO
public record CreateNoteDto(
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
    string Title,
    string? Content = null,
    string Category = "Personal"
);

// -- Update DTO
public record UpdateNoteDto(
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
    string Title,
    string? Content = null,
    string? Category = null
);

// -- Note Detail DTO
public record NoteDetailDto(
    int Id,
    string Title,
    string? Content,
    string Category,
    int UserId,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

// -- Search, Filter & Sort Query Parameters
public class NoteQueryDto
{
    public string? Search { get; set; }          
    public string? Category { get; set; }        
    public string? SortOrder { get; set; } = "desc"; 
}
