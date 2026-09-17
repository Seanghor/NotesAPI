using NotesApi.DTOs;

namespace NotesApi.Services;

public interface INoteService
{
    Task<IEnumerable<NoteDetailDto>> GetNotesForUser(int userId, NoteQueryDto query);
    Task<NoteDetailDto?> GetNoteByIdForUser(int id, int userId);
    Task<NoteDetailDto> CreateNote(CreateNoteDto dto, int userId);
    Task<bool> UpdateNote(int id, UpdateNoteDto dto, int userId);
    Task<bool> DeleteNote(int id, int userId);
}
