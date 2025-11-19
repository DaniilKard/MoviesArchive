using Microsoft.EntityFrameworkCore;
using MoviesArchive.Data.Context;
using MoviesArchive.Data.Enums;
using MoviesArchive.Data.Extensions;
using MoviesArchive.Data.Interfaces;
using MoviesArchive.Data.Models;

namespace MoviesArchive.Data.Repositories;

internal class MovieRepository : IMovieRepository
{
    private readonly ApplicationContext _db;

    public MovieRepository(ApplicationContext database) 
    { 
        _db = database;
    }

    public async Task<int> GetMoviePageCount(int searchGenreId, string searchLine)
    {
        var count = await _db.Movies
            .AsNoTracking()
            .Where(m =>
                (searchGenreId == 0 || m.GenreId == searchGenreId) && 
                (searchLine == null || m.Title.ToLower().Contains(searchLine.ToLower())))
            .CountAsync();
        return count;
    }

    public async Task<int> GetMoviePageCount(int searchGenreId, string searchLine, int userId)
    {
        var count = await _db.Movies
            .AsNoTracking()
            .Where(m => 
                (m.UserId == userId) && 
                (searchGenreId == 0 || m.GenreId == searchGenreId) &&
                (searchLine == null || m.Title.ToLower().Contains(searchLine.ToLower())))
            .CountAsync();
        return count;
    }

    public async Task<Movie> GetMovie(int id)
    {
        var movie = await _db.Movies.FirstOrDefaultAsync(m => m.Id == id);
        return movie;
    }

    public async Task<List<Movie>> GetMoviesByGenre(int genreId)
    {
        var movies = await _db.Movies.Where(m => m.GenreId == genreId).ToListAsync();
        return movies;
    }

    public async Task<List<string>> GetMovieTitlesForUser(int userId)
    {
        var titles = await _db.Movies.AsNoTracking().Where(m => m.UserId == userId).Select(m => m.Title).ToListAsync();
        return titles;
    }

    public async Task<List<Movie>> GetSortedMovies(MovieSort sort, int pageNum, int elementsOnPage, int searchGenreId, string searchLine)
    {
        var sortedMovies = await _db.Movies
            .AsNoTracking()
            .Include(m => m.Genre)
            .Where(m => 
                (searchGenreId == 0 || m.GenreId == searchGenreId) &&
                (searchLine == null || m.Title.ToLower().Contains(searchLine.ToLower())))
            .OrderMovies(sort)
            .Skip((pageNum - 1) * elementsOnPage)
            .Take(elementsOnPage)
            .ToListAsync();
        return sortedMovies;
    }

    public async Task<List<Movie>> GetSortedMoviesByUser(MovieSort sort, int pageNum, int elementsOnPage, int searchGenreId, string searchLine, int userId)
    {
        var sortedMovies = await _db.Movies
            .AsNoTracking()
            .Include(m => m.Genre)
            .Where(m => 
                (m.UserId == userId) && 
                (searchGenreId == 0 || m.GenreId == searchGenreId) &&
                (searchLine == null || m.Title.ToLower().Contains(searchLine.ToLower())))
            .OrderMovies(sort)
            .Skip((pageNum - 1) * elementsOnPage)
            .Take(elementsOnPage)
            .ToListAsync();
        return sortedMovies;
    }

    public async Task<int> AddMovie(Movie movie)
    {
        _db.Movies.Add(movie);
        var result = await _db.SaveChangesAsync();
        return result;
    }

    public async Task<int> AddMovieRange(List<Movie> movies)
    {
        _db.Movies.AddRange(movies);
        var result = await _db.SaveChangesAsync();
        return result;
    }

    public async Task<int> UpdateMovie(Movie movie)
    {
        _db.Movies.Update(movie);
        var result = await _db.SaveChangesAsync();
        return result;
    }

    public async Task<int> UpdateMovieRange(List<Movie> movies)
    {
        _db.Movies.UpdateRange(movies);
        var result = await _db.SaveChangesAsync();
        return result;
    }

    public async Task<int> RemoveMovie(Movie movie)
    {
        _db.Movies.Remove(movie);
        var result = await _db.SaveChangesAsync();
        return result;
    }

    public async Task<bool> MovieExists(Movie movie)
    {
        var movieExists = await _db.Movies.AnyAsync(m => m.UserId == movie.UserId && m.Title.ToLower() == movie.Title.ToLower());
        return movieExists;
    }
}
