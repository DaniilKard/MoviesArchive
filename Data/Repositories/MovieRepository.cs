using Microsoft.EntityFrameworkCore;
using MoviesArchive.Data.Context;
using MoviesArchive.Data.Enums;
using MoviesArchive.Data.Interfaces;
using MoviesArchive.Data.Models;
using Serilog;

namespace MoviesArchive.Data.Repositories;

internal class MovieRepository : IMovieRepository
{
    private readonly ApplicationContext _db;

    public MovieRepository(ApplicationContext database) 
    { 
        _db = database;
    }

    public async Task<int> GetMovieCount()
    {
        var count = await _db.Movies.AsNoTracking().CountAsync();
        return count;
    }

    public async Task<int> GetMovieCount(int userId)
    {
        var count = await _db.Movies.AsNoTracking().Where(m => m.UserId == userId).CountAsync();
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

    public async Task<List<Movie>> GetSortedMovies(MovieSort sort, int pageNum, int elementsOnPage)
    {
        var sortedMovies = sort switch
        {
            MovieSort.TitleAsc =>        await _db.Movies.AsNoTracking().Include(m => m.Genre)
                .OrderBy(m => m.Title).Skip((pageNum - 1) * elementsOnPage).Take(elementsOnPage).ToListAsync(),
            MovieSort.TitleDesc =>       await _db.Movies.AsNoTracking().Include(m => m.Genre)
                .OrderByDescending(m => m.Title).Skip((pageNum - 1) * elementsOnPage).Take(elementsOnPage).ToListAsync(),
            MovieSort.GenreAsc =>        await _db.Movies.AsNoTracking().Include(m => m.Genre)
                .OrderBy(m => m.Genre.Name).ThenBy(m => m.Title).Skip((pageNum - 1) * elementsOnPage).Take(elementsOnPage).ToListAsync(),
            MovieSort.GenreDesc =>       await _db.Movies.AsNoTracking().Include(m => m.Genre)
                .OrderByDescending(m => m.Genre.Name).ThenBy(m => m.Title).Skip((pageNum - 1) * elementsOnPage).Take(elementsOnPage).ToListAsync(),
            MovieSort.RatingAsc =>       await _db.Movies.AsNoTracking().Include(m => m.Genre)
                .OrderBy(m => m.Rating == null).ThenBy(m => m.Rating).Skip((pageNum - 1) * elementsOnPage).Take(elementsOnPage).ToListAsync(),
            MovieSort.RatingDesc =>      await _db.Movies.AsNoTracking().Include(m => m.Genre)
                .OrderBy(m => m.Rating == null).ThenByDescending(m => m.Rating).Skip((pageNum - 1) * elementsOnPage).Take(elementsOnPage).ToListAsync(),
            MovieSort.ReleaseYearAsc =>  await _db.Movies.AsNoTracking().Include(m => m.Genre)
                .OrderBy(m => m.ReleaseYear == null).ThenBy(m => m.ReleaseYear).Skip((pageNum - 1) * elementsOnPage).Take(elementsOnPage).ToListAsync(),
            MovieSort.ReleaseYearDesc => await _db.Movies.AsNoTracking().Include(m => m.Genre)
                .OrderBy(m => m.ReleaseYear == null).ThenByDescending(m => m.ReleaseYear).Skip((pageNum - 1) * elementsOnPage).Take(elementsOnPage).ToListAsync(),
            MovieSort.UserIdDesc =>      await _db.Movies.AsNoTracking().Include(m => m.Genre)
                .OrderByDescending(m => m.UserId).ThenBy(m => m.Title).Skip((pageNum - 1) * elementsOnPage).Take(elementsOnPage).ToListAsync(),
            _ =>                         await _db.Movies.AsNoTracking().Include(m => m.Genre)
                .OrderBy(m => m.UserId).ThenBy(m => m.Title).Skip((pageNum - 1) * elementsOnPage).Take(elementsOnPage).ToListAsync(),
        };
        return sortedMovies;
    }

    public async Task<List<Movie>> GetSortedMoviesByUser(MovieSort sort, int pageNum, int elementsOnPage, int userId)
    {
        var sortedMovies = sort switch
        {
            MovieSort.TitleDesc =>       await _db.Movies.AsNoTracking().Include(m => m.Genre).Where(m => m.UserId == userId)
                .OrderByDescending(m => m.Title).Skip((pageNum - 1) * elementsOnPage).Take(elementsOnPage).ToListAsync(),
            MovieSort.GenreAsc =>        await _db.Movies.AsNoTracking().Include(m => m.Genre).Where(m => m.UserId == userId)
                .OrderBy(m => m.Genre.Name).ThenBy(m => m.Title).Skip((pageNum - 1) * elementsOnPage).Take(elementsOnPage).ToListAsync(),
            MovieSort.GenreDesc =>       await _db.Movies.AsNoTracking().Include(m => m.Genre).Where(m => m.UserId == userId)
                .OrderByDescending(m => m.Genre.Name).ThenBy(m => m.Title).Skip((pageNum - 1) * elementsOnPage).Take(elementsOnPage).ToListAsync(),
            MovieSort.RatingAsc =>       await _db.Movies.AsNoTracking().Include(m => m.Genre).Where(m => m.UserId == userId)
                .OrderBy(m => m.Rating == null).ThenBy(m => m.Rating).Skip((pageNum - 1) * elementsOnPage).Take(elementsOnPage).ToListAsync(),
            MovieSort.RatingDesc =>      await _db.Movies.AsNoTracking().Include(m => m.Genre).Where(m => m.UserId == userId)
                .OrderBy(m => m.Rating == null).ThenByDescending(m => m.Rating).Skip((pageNum - 1) * elementsOnPage).Take(elementsOnPage).ToListAsync(),
            MovieSort.ReleaseYearAsc =>  await _db.Movies.AsNoTracking().Include(m => m.Genre).Where(m => m.UserId == userId)
                .OrderBy(m => m.ReleaseYear == null).ThenBy(m => m.ReleaseYear).Skip((pageNum - 1) * elementsOnPage).Take(elementsOnPage).ToListAsync(),
            MovieSort.ReleaseYearDesc => await _db.Movies.AsNoTracking().Include(m => m.Genre).Where(m => m.UserId == userId)
                .OrderBy(m => m.ReleaseYear == null).ThenByDescending(m => m.ReleaseYear).Skip((pageNum - 1) * elementsOnPage).Take(elementsOnPage).ToListAsync(),
            _ =>                         await _db.Movies.AsNoTracking().Include(m => m.Genre).Where(m => m.UserId == userId)
                .OrderBy(m => m.Title).Skip((pageNum - 1) * elementsOnPage).Take(elementsOnPage).ToListAsync(),
        };
        return sortedMovies;
    }

    public async Task<ResultStatus> AddMovie(Movie movie)
    {
        _db.Movies.Add(movie);
        var result = await _db.SaveChangesAsync();
        if (result == 0)
        {
            Log.Warning("AddMovieAsync has written 0 state entries");
            return ResultStatus.Failed;
        }
        return ResultStatus.Success;
    }

    public async Task<ResultStatus> AddMovieRange(List<Movie> movies)
    {
        _db.Movies.AddRange(movies);
        var result = await _db.SaveChangesAsync();
        if (result == 0)
        {
            Log.Warning("AddMovieRange has written 0 state entries");
            return ResultStatus.Failed;
        }
        return ResultStatus.Success;
    }

    public async Task<ResultStatus> UpdateMovie(Movie movie)
    {
        _db.Movies.Update(movie);
        var result = await _db.SaveChangesAsync();
        if (result == 0)
        {
            Log.Warning("UpdateMovieAsync has written 0 state entries");
            return ResultStatus.Failed;
        }
        return ResultStatus.Success;
    }

    public async Task UpdateMovieRange(List<Movie> movies)
    {
        _db.Movies.UpdateRange(movies);
        var result = await _db.SaveChangesAsync();
        if (result == 0)
        {
            Log.Warning("UpdateMovieRangeAsync has written 0 state entries");
        }
    }

    public async Task<ResultStatus> RemoveMovie(Movie movie)
    {
        _db.Movies.Remove(movie);
        var result = await _db.SaveChangesAsync();
        if (result == 0)
        {
            Log.Warning("RemoveMovieAsync has written 0 state entries");
            return ResultStatus.Failed;
        }
        return ResultStatus.Success;
    }

    public async Task<bool> MovieExists(Movie movie)
    {
        var movieExists = await _db.Movies.AnyAsync(m => m.UserId == movie.UserId && m.Title.ToLower() == movie.Title.ToLower());
        return movieExists;
    }
}
