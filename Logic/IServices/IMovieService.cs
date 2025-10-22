using MoviesArchive.Data.Enums;
using MoviesArchive.Data.Models;
using MoviesArchive.Logic.ModelsDto;

namespace MoviesArchive.Logic.IServices;

public interface IMovieService
{
    Task<Movie> GetMovie(int id);
    Task<MovieIndexDto> GetCurrentMovieIndex(MovieSort sort, int currentPage);
    Task<MovieIndexDto> GetCurrentMovieIndex(MovieSort sort, int currentPage, int userId);
    Task<ResultStatus> AddMovie(Movie movie);
    Task<ResultStatus> AddFileToDatabase(int userId);
    Task<ResultStatus> UpdateMovie(Movie movie);
    Task<ResultStatus> RemoveMovie(Movie movie);
}
