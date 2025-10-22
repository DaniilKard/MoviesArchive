using MoviesArchive.Data.Enums;
using MoviesArchive.Data.Models;

namespace MoviesArchive.Data.Interfaces;

public interface IMovieRepository
{
    Task<int> GetMovieCount();
    Task<int> GetMovieCount(int userId);
    Task<Movie> GetMovie(int id);
    Task<List<Movie>> GetSortedMovies(MovieSort sort, int pageNum, int elementsOnPage);
    Task<List<Movie>> GetSortedMoviesByUser(MovieSort sort, int pageNum, int elementsOnPage, int userId);
    Task<List<string>> GetMovieTitlesForUser(int userId);
    Task<List<Movie>> GetMoviesByGenre(int genreId);
    Task<int> AddMovie(Movie movie);
    Task<int> AddMovieRange(List<Movie> movies);
    Task<int> UpdateMovie(Movie movie);
    Task UpdateMovieRange(List<Movie> movies);
    Task<int> RemoveMovie(Movie movie);
    Task<bool> MovieExists(Movie movie);
}
