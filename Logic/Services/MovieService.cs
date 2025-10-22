using Mapster;
using Microsoft.Extensions.Configuration;
using MoviesArchive.Data.Enums;
using MoviesArchive.Data.Interfaces;
using MoviesArchive.Data.Models;
using MoviesArchive.Logic.IServices;
using MoviesArchive.Logic.Models;
using MoviesArchive.Logic.ModelsDto;
using MoviesArchive.Logic.Parsers;
using Serilog;

namespace MoviesArchive.Logic.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IConfiguration _config;
    private IFileParser Parser { get; set; }

    public MovieService(IMovieRepository movieRepository, IGenreRepository genreRepository, IConfiguration config, IFileParser fileParser)
    {
        _movieRepository = movieRepository;
        _genreRepository = genreRepository;
        _config = config;
        Parser = fileParser;
    }

    public async Task<Movie> GetMovie(int id)
    {
        return await _movieRepository.GetMovie(id);
    }

    public async Task<MovieIndexDto> GetCurrentMovieIndex(MovieSort sort, int currentPage)
    {
        var elementsOnPage = _config.GetValue<int>("ElementsOnOnePage");
        var movieCount = await _movieRepository.GetMovieCount();
        var pagesCount = (int)Math.Ceiling(movieCount / (double)elementsOnPage);

        var moviesWithGenres = await _movieRepository.GetSortedMovies(sort, currentPage, elementsOnPage);
        var movies = moviesWithGenres.Select(m => m.Adapt<MovieDto>()).ToList();

        var movieIndexDto = new MovieIndexDto
        {
            Sort = sort,
            CurrentPage = currentPage,
            PagesCount = pagesCount,
            Movies = movies
        };
        return movieIndexDto;
    }

    public async Task<MovieIndexDto> GetCurrentMovieIndex(MovieSort sort, int currentPage, int userId)
    {
        var elementsOnPage = _config.GetValue<int>("ElementsOnOnePage");
        var movieCount = await _movieRepository.GetMovieCount(userId);
        var pagesCount = (int)Math.Ceiling(movieCount / (double)elementsOnPage);

        var moviesWithGenres = await _movieRepository.GetSortedMoviesByUser(sort, currentPage, elementsOnPage, userId);
        var movies = moviesWithGenres.Select(m => m.Adapt<MovieDto>()).ToList();

        var movieIndexDto = new MovieIndexDto
        {
            Sort = sort,
            CurrentPage = currentPage,
            PagesCount = pagesCount,
            Movies = movies
        };
        return movieIndexDto;
    }

    public async Task<ResultStatus> AddMovie(Movie movie)
    {
        movie.Title = movie.Title.Trim();
        var movieExists = await _movieRepository.MovieExists(movie);
        if (!movieExists)
        {
            var result = await _movieRepository.AddMovie(movie);
            return result == 0 ? ResultStatus.Failed : ResultStatus.Success;
        }
        return ResultStatus.Failed;
    }

    public async Task<ResultStatus> AddFileToDatabase(int userId)
    {
        var filePath = _config.GetValue<string>("MoviesFilePath");
        if (filePath is null)
        {
            return ResultStatus.NotFound;
        }

        try
        {
            var files = Directory.GetFiles(filePath, "*.md", SearchOption.TopDirectoryOnly);
            if (files.Length != 1)
            {
                files = Directory.GetFiles(filePath, "*.docx", SearchOption.TopDirectoryOnly);
            }
            if (files.Length == 1)
            {
                Parser = files.Single().EndsWith(".md") ? new MdFileParser() : new DocxFileParser();
                var parsedMovies = Parser.ParseFile(files.Single());
                var movieTitlesFromDb = await _movieRepository.GetMovieTitlesForUser(userId);
                var moviesToAdd = parsedMovies.Where(m => !movieTitlesFromDb.Contains(m.Title)).ToList();
                if (moviesToAdd.Count != 0)
                {
                    var genres = await _genreRepository.GetGenresList();
                    foreach (var movie in moviesToAdd)
                    {
                        movie.Genre = genres.SingleOrDefault(g => g.Name == movie.Genre?.Name);
                        movie.UserId = userId;
                        movie.Comment = movie.Comment ?? "";
                    }
                    var result = await _movieRepository.AddMovieRange(moviesToAdd);
                    return result == 0 ? ResultStatus.Failed : ResultStatus.Success;
                }
            }
            else if (files.Length > 1)
            {
                Log.Warning("Folder contains multiple .md or .docx files");
            }
            else
            {
                Log.Warning("Folder contains no .md or .docx files");
                return ResultStatus.NotFound;
            }
        }
        catch (Exception ex)
        {
            Log.Error($"AddFileToDatabase error: {ex.Message}");
            return ResultStatus.NotFound;
        }
        return ResultStatus.Failed;
    }

    public async Task<ResultStatus> UpdateMovie(Movie movie)
    {
        var result = await _movieRepository.UpdateMovie(movie);
        return result == 0 ? ResultStatus.Failed : ResultStatus.Success;
    }

    public async Task<ResultStatus> RemoveMovie(Movie movie)
    {
        var result = await _movieRepository.RemoveMovie(movie);
        return result == 0 ? ResultStatus.Failed : ResultStatus.Success;
    }
}