using MoviesArchive.Data.Enums;
using MoviesArchive.Data.Models;

namespace MoviesArchive.Logic.IServices;

public interface IGenreService
{
    Task<Genre> GetGenreById(int id);
    Task<List<Genre>> GetGenresList();
    Task<ResultStatus> AddGenre(Genre genre);
    Task<ResultStatus> UpdateGenre(Genre genre);
    Task<ResultStatus> RemoveGenre(Genre genre);
}
