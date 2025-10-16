using MoviesArchive.Data.Enums;
using MoviesArchive.Data.Interfaces;
using MoviesArchive.Data.Models;
using MoviesArchive.Logic.IServices;

namespace MoviesArchive.Logic.Services;

internal class GenreService : IGenreService
{
    private readonly IGenreRepository _genreRepository;
    private readonly IMovieRepository _movieRepository;

    public GenreService(IGenreRepository genreRepository, IMovieRepository movieRepository)
    {
        _genreRepository = genreRepository;
        _movieRepository = movieRepository;
    }

    public async Task<Genre> GetGenreById(int id)
    {
        return await _genreRepository.GetGenreById(id);
    }

    public async Task<List<Genre>> GetGenresList()
    {
        var genres = await _genreRepository.GetGenresList();
        var orderedGenres = genres.Where(g => g.Name != "undefined").OrderBy(g => g.Name).ToList();
        return orderedGenres;
    }

    public async Task<ResultStatus> AddGenre(Genre genre)
    {
        var resultStatus = ResultStatus.Failed;
        var genres = await _genreRepository.GetGenresListAsNoTracking();
        if (!genres.Any(g => g.Name.Equals(genre.Name, StringComparison.CurrentCultureIgnoreCase)))
        {
            resultStatus = await _genreRepository.AddGenre(genre);
        }
        return resultStatus;
    }

    public async Task<ResultStatus> UpdateGenre(Genre genre)
    {
        var resultStatus = ResultStatus.Failed;
        var genres = await _genreRepository.GetGenresListAsNoTracking();
        if (!genres.Any(g => g.Name.Equals(genre.Name, StringComparison.CurrentCultureIgnoreCase)))
        {
            resultStatus = await _genreRepository.UpdateGenre(genre);
        }
        return resultStatus;
    }

    public async Task<ResultStatus> RemoveGenre(Genre genre)
    {
        var resultStatus = ResultStatus.Failed;
        var defaultGenre = await _genreRepository.GetGenreByName("undefined");
        var genreMovies = await _movieRepository.GetMoviesByGenre(genre.Id);
        if (genreMovies is not null && genreMovies.Count != 0)
        {
            genreMovies.ForEach(m => m.GenreId = defaultGenre.Id);
            await _movieRepository.UpdateMovieRange(genreMovies);
        }
        resultStatus = await _genreRepository.RemoveGenre(genre);
        return resultStatus;
    }
}
