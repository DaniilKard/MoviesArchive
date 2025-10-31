using MoviesArchive.Data.Enums;
using MoviesArchive.Data.Interfaces;
using MoviesArchive.Data.Models;
using MoviesArchive.Logic.IServices;
using Serilog;

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
        var genres = await _genreRepository.GetGenresListAsNoTracking();
        var genreExists = genres.Any(g => g.Name.Equals(genre.Name, StringComparison.CurrentCultureIgnoreCase));
        if (!genreExists)
        {
            var result = await _genreRepository.AddGenre(genre);
            if (result == 0)
            {
                Log.Warning($"{nameof(_genreRepository.AddGenre)} has written 0 state entries");
            }
            return result == 0 ? ResultStatus.Failed : ResultStatus.Success;
        }
        return ResultStatus.Failed;
    }

    public async Task<ResultStatus> UpdateGenre(Genre genre)
    {
        var genres = await _genreRepository.GetGenresListAsNoTracking();
        var genreExists = genres.Any(g => g.Name.Equals(genre.Name, StringComparison.CurrentCultureIgnoreCase));
        if (!genreExists)
        {
            var result = await _genreRepository.UpdateGenre(genre);
            if (result == 0)
            {
                Log.Warning($"{nameof(_genreRepository.UpdateGenre)} has written 0 state entries");
            }
            return result == 0 ? ResultStatus.Failed : ResultStatus.Success;
        }
        return ResultStatus.Failed;
    }

    public async Task<ResultStatus> RemoveGenre(Genre genre)
    {
        var defaultGenre = await _genreRepository.GetGenreByName("undefined");
        var genreMovies = await _movieRepository.GetMoviesByGenre(genre.Id);
        if (genreMovies.Count != 0)
        {
            genreMovies.ForEach(m => m.GenreId = defaultGenre.Id);
            var updateResult = await _movieRepository.UpdateMovieRange(genreMovies);
            if (updateResult == 0)
            {
                Log.Warning($"{nameof(_movieRepository.UpdateMovieRange)} has written 0 state entries");
            }
        }
        var result = await _genreRepository.RemoveGenre(genre);
        if (result == 0)
        {
            Log.Warning($"{nameof(_genreRepository.RemoveGenre)} has written 0 state entries");
        }
        return result == 0 ? ResultStatus.Failed : ResultStatus.Success;
    }
}
