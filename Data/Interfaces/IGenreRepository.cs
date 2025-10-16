using MoviesArchive.Data.Enums;
using MoviesArchive.Data.Models;

namespace MoviesArchive.Data.Interfaces;

public interface IGenreRepository
{
    Task<Genre> GetGenreById(int id);
    Task<Genre> GetGenreByName(string name);
    Task<List<Genre>> GetGenresList();
    Task<List<Genre>> GetGenresListAsNoTracking();
    Task<ResultStatus> AddGenre(Genre genre);
    Task<ResultStatus> UpdateGenre(Genre genre);
    Task<ResultStatus> RemoveGenre(Genre genre);
}
