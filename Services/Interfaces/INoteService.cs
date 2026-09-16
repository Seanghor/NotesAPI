using NotesApi.DTOs;

namespace NotesApi.Services.Interfaces;

public interface INoteService
{
    Task<IEnumerable<NoteDetailDto>> GetNotesForUserAsync(int userId, NoteQueryDto query);
    Task<NoteDetailDto?> GetNoteByIdForUserAsync(int id, int userId);
    Task<NoteDetailDto> CreateNoteAsync(CreateNoteDto dto, int userId);
    Task<bool> UpdateNoteAsync(int id, UpdateNoteDto dto, int userId);
    Task<bool> DeleteNoteAsync(int id, int userId);
}
