using NotesApi.Models;

namespace NotesApi.Repositories.Interfaces;

public interface INoteRepository
{
    Task<IEnumerable<Note>> GetAllByUserAsync(int userId, string? search, DateTime? fromDate, DateTime? toDate, string? sortOrder);
    Task<Note?> GetByIdAndUserAsync(int id, int userId);
    Task<int> CreateAsync(Note note);
    Task<bool> UpdateAsync(Note note);
    Task<bool> DeleteAsync(int id, int userId);
}
