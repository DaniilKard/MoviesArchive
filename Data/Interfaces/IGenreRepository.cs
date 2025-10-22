using MoviesArchive.Data.Models;

namespace MoviesArchive.Data.Interfaces;

public interface IGenreRepository
{
    Task<Genre> GetGenreById(int id);
    Task<Genre> GetGenreByName(string name);
    Task<List<Genre>> GetGenresList();
    Task<List<Genre>> GetGenresListAsNoTracking();
    Task<int> AddGenre(Genre genre);
    Task<int> UpdateGenre(Genre genre);
    Task<int> RemoveGenre(Genre genre);
}
