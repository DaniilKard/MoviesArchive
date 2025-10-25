using MoviesArchive.Data.Enums;
using MoviesArchive.Data.Models;

namespace MoviesArchive.Data.Extensions;

public static class QueryableExtension
{
    public static IQueryable<Movie> OrderMovies(this IQueryable<Movie> query, MovieSort movieSort)
    {
        var sortedMovies = movieSort switch
        {
            MovieSort.TitleDesc => query.OrderByDescending(m => m.Title),
            MovieSort.GenreAsc => query.OrderBy(m => m.Genre.Name).ThenBy(m => m.Title),
            MovieSort.GenreDesc => query.OrderByDescending(m => m.Genre.Name).ThenBy(m => m.Title),
            MovieSort.RatingAsc => query.OrderBy(m => m.Rating == null).ThenBy(m => m.Rating),
            MovieSort.RatingDesc => query.OrderBy(m => m.Rating == null).ThenByDescending(m => m.Rating),
            MovieSort.ReleaseYearAsc => query.OrderBy(m => m.ReleaseYear == null).ThenBy(m => m.ReleaseYear),
            MovieSort.ReleaseYearDesc => query.OrderBy(m => m.ReleaseYear == null).ThenByDescending(m => m.ReleaseYear),
            _ => query.OrderBy(m => m.Title),
        };
        return sortedMovies;
    }
}
