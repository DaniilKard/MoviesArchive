using Mapster;
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
    private IFileParser Parser { get; set; }

    public MovieService(IMovieRepository movieRepository, IGenreRepository genreRepository)
    {
        _movieRepository = movieRepository;
        _genreRepository = genreRepository;
    }

    public async Task<Movie> GetMovie(int id)
    {
        return await _movieRepository.GetMovie(id);
    }

    public async Task<MovieEditDto> GetMovieEditDto(int id)
    {
        var movie = await _movieRepository.GetMovie(id);
        var movieEditDto = movie.Adapt<MovieEditDto>();

        var genres = await _genreRepository.GetGenresList();
        var orderedGenres = genres.Where(g => g.Name != "undefined").OrderBy(g => g.Name).ToList();
        movieEditDto.Genres = orderedGenres;
        return movieEditDto;
    }

    public async Task<MovieIndexDto> GetIndexPageMovies(MovieSort sort, int currentPage, int searchGenreId, string searchLine, int userId = -1)
    {
        var elementsOnPage = Global.ElementsOnOnePage;

        var movieCount = userId == -1 ? 
            await _movieRepository.GetMoviePageCount(searchGenreId, searchLine) : 
            await _movieRepository.GetMoviePageCount(searchGenreId, searchLine, userId);
        var moviesWithGenres = userId == -1 ? 
            await _movieRepository.GetSortedMovies(sort, currentPage, elementsOnPage, searchGenreId, searchLine) : 
            await _movieRepository.GetSortedMoviesByUser(sort, currentPage, elementsOnPage, searchGenreId, searchLine, userId);
        var genres = await _genreRepository.GetGenresListAsNoTracking();
        var orderedGenres = genres.Where(g => g.Name != "undefined").OrderBy(g => g.Name).ToList();
        
        var pagesCount = (int)Math.Ceiling(movieCount / (double)elementsOnPage);
        var movies = moviesWithGenres.Select(m => m.Adapt<MovieDto>()).ToList();
        var movieIndexDto = new MovieIndexDto
        {
            Sort = sort,
            CurrentPage = currentPage,
            PagesCount = pagesCount,
            Movies = movies,
            Genres = orderedGenres,
            SearchGenreId = searchGenreId,
            SearchLine = searchLine
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
        var filePath = Global.MoviesFilePath;
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