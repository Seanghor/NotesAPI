using NotesApi.DTOs;
using NotesApi.Models;
using NotesApi.Repositories;

namespace NotesApi.Services;

public class NoteService : INoteService
{
    private readonly INoteRepository _repository;

    public NoteService(INoteRepository repository){
        _repository = repository;
    }

    // Get Notes of User
    public async Task<IEnumerable<NoteDetailDto>> GetNotesForUser(int userId, NoteQueryDto query){
        var notes = await _repository.GetAllByUserAsync(
            userId: userId,
            search: query.Search,
            fromDate: query.FromDate,
            toDate: query.ToDate,
            sortOrder: query.SortOrder
        );

        return notes.Select(note => new NoteDetailDto(
            note.Id,
            note.Title,
            note.Content,
            note.UserId,
            note.CreatedAt,
            note.UpdatedAt
        ));
    }

    // Get One by ID
    public async Task<NoteDetailDto?> GetNoteByIdForUser(int id, int userId){
        var note = await _repository.GetByIdAndUserAsync(id, userId);
        if (note == null) return null;

        return new NoteDetailDto(
            note.Id,
            note.Title,
            note.Content,
            note.UserId,
            note.CreatedAt,
            note.UpdatedAt
        );
    }

    // Create note
    public async Task<NoteDetailDto> CreateNote(CreateNoteDto userDto, int userId){
        var note = new Note{
            Title = userDto.Title,
            Content = userDto.Content ?? string.Empty,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };

        var generatedId = await _repository.CreateAsync(note);
        note.Id = generatedId;

        return new NoteDetailDto(
            note.Id,
            note.Title,
            note.Content,
            note.UserId,
            note.CreatedAt,
            note.UpdatedAt
        );
    }

    // Update by ID
    public async Task<bool> UpdateNote(int id, UpdateNoteDto dto, int userId){
        var existingNote = await _repository.GetByIdAndUserAsync(id, userId);
        if (existingNote == null) return false;

        existingNote.Title = dto.Title;
        existingNote.Content = dto.Content ?? string.Empty;
        existingNote.UpdatedAt = DateTime.UtcNow;

        return await _repository.UpdateAsync(existingNote);
    }

    //Delete by ID
    public async Task<bool> DeleteNote(int id, int userId){
        return await _repository.DeleteAsync(id, userId);
    }
}
