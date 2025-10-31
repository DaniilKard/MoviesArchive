using Microsoft.EntityFrameworkCore;
using MoviesArchive.Data.Context;
using MoviesArchive.Data.Interfaces;
using MoviesArchive.Data.Models;

namespace MoviesArchive.Data.Repositories;

internal class GenreRepository : IGenreRepository
{
    private readonly ApplicationContext _db;

    public GenreRepository(ApplicationContext database)
    {
        _db = database;
    }

    public async Task<Genre> GetGenreById(int id)
    {
        var genre = await _db.Genres.FirstOrDefaultAsync(g => g.Id == id);
        return genre;
    }

    public async Task<Genre> GetGenreByName(string name)
    {
        var genre = await _db.Genres.FirstOrDefaultAsync(g => g.Name == name);
        return genre;
    }

    public async Task<List<Genre>> GetGenresList()
    {
        var genres = await _db.Genres.ToListAsync();
        return genres;
    }

    public async Task<List<Genre>> GetGenresListAsNoTracking()
    {
        var genres = await _db.Genres.AsNoTracking().ToListAsync();
        return genres;
    }

    public async Task<int> AddGenre(Genre genre)
    {
        _db.Genres.Add(genre);
        var result = await _db.SaveChangesAsync();
        return result;
    }

    public async Task<int> UpdateGenre(Genre genre)
    {
        _db.Genres.Update(genre);
        var result = await _db.SaveChangesAsync();        
        return result;
    }

    public async Task<int> RemoveGenre(Genre genre)
    {
        _db.Genres.Remove(genre);
        var result = await _db.SaveChangesAsync();
        return result;
    }
}
