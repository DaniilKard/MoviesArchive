using MoviesArchive.Data.Enums;
using MoviesArchive.Data.Models;
using MoviesArchive.Logic.ModelsDto;

namespace MoviesArchive.Logic.IServices;

public interface IMovieService
{
    Task<Movie> GetMovie(int id);
    Task<MovieEditDto> GetMovieEditDto(int id);
    Task<MovieIndexDto> GetIndexPageMovies(MovieSort sort, int currentPage, int userId = -1);
    Task<ResultStatus> AddMovie(Movie movie);
    Task<ResultStatus> AddFileToDatabase(int userId);
    Task<ResultStatus> UpdateMovie(Movie movie);
    Task<ResultStatus> RemoveMovie(Movie movie);
}
