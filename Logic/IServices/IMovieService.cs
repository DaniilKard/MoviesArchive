using MoviesArchive.Data.Enums;
using MoviesArchive.Data.Models;
using MoviesArchive.Logic.Models;

namespace MoviesArchive.Logic.IServices;

public interface IMovieService
{
    Task<Movie> GetMovie(int id);
    Task<int> GetPageCount(int elementsOnPage);
    Task<int> GetPageCount(int elementsOnPage, int userId);
    Task<List<MovieDto>> GetSortedMovies(MovieSort sort, int pageNum, int elementsOnPage);
    Task<List<MovieDto>> GetSortedMovies(MovieSort sort, int pageNum, int elementsOnPage, int userId);
    Task<ResultStatus> AddMovie(Movie movie);
    Task<ResultStatus> AddFileToDatabase(string filePath, int userId);
    Task<ResultStatus> UpdateMovie(Movie movie);
    Task<ResultStatus> RemoveMovie(Movie movie);
}
