using Mapster;
using MoviesArchive.Data.Enums;
using MoviesArchive.Data.Interfaces;
using MoviesArchive.Data.Models;
using MoviesArchive.Logic.IServices;
using MoviesArchive.Logic.Models;
using MoviesArchive.Logic.Parsers;
using Serilog;

namespace MoviesArchive.Logic.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IGenreRepository _genreRepository;
    private IFileParser Parser { get; set; }

    public MovieService(IMovieRepository movieRepository, IGenreRepository genreRepository, IFileParser fileParser)
    {
        _movieRepository = movieRepository;
        _genreRepository = genreRepository;
        Parser = fileParser;
    }

    public async Task<Movie> GetMovie(int id)
    {
        return await _movieRepository.GetMovie(id);
    }

    public async Task<int> GetPageCount(int elementsOnPage)
    {
        var movieCount = await _movieRepository.GetMovieCount();
        var totalPageCount = (int)Math.Ceiling(movieCount / (double)elementsOnPage);
        return totalPageCount;
    }

    public async Task<int> GetPageCount(int elementsOnPage, int userId)
    {
        var movieCount = await _movieRepository.GetMovieCount(userId);
        var totalPageCount = (int)Math.Ceiling(movieCount / (double)elementsOnPage);
        return totalPageCount;
    }

    public async Task<List<MovieDto>> GetSortedMovies(MovieSort sort, int pageNum, int elementsOnPage)
    {
        var moviesWithGenres = await _movieRepository.GetSortedMovies(sort, pageNum, elementsOnPage);
        var moviesDto = moviesWithGenres.Select(m => m.Adapt<MovieDto>()).ToList();
        return moviesDto;
    }

    public async Task<List<MovieDto>> GetSortedMovies(MovieSort sort, int pageNum, int elementsOnPage, int userId)
    {
        var moviesWithGenres = await _movieRepository.GetMoviesForIndex(sort, pageNum, elementsOnPage, userId);
        var moviesDto = moviesWithGenres.Select(m => m.Adapt<MovieDto>()).ToList();
        return moviesDto;
    }

    public async Task<ResultStatus> AddMovie(Movie movie)
    {
        movie.Title = movie.Title.Trim();
        var movieExists = await _movieRepository.MovieExists(movie);
        var result = ResultStatus.Failed;
        if (!movieExists)
        {
            result = await _movieRepository.AddMovie(movie);
        }
        return result;
    }

    public async Task<ResultStatus> AddFileToDatabase(string filePath, int userId)
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
                var resultStatus = await _movieRepository.AddMovieRange(moviesToAdd);
                return resultStatus;
            }
        }
        else if (files.Length > 1)
        {
            Log.Warning("Folder contains multiple .md or .docx files");
        }
        else
        {
            Log.Warning("Folder contains no .md or .docx files");
        }
        return ResultStatus.Failed;
    }

    public async Task<ResultStatus> UpdateMovie(Movie movie)
    {
        var resultStatus = await _movieRepository.UpdateMovie(movie);
        return resultStatus;
    }

    public async Task<ResultStatus> RemoveMovie(Movie movie)
    {
        var result = await _movieRepository.RemoveMovie(movie);
        return result;
    }
}