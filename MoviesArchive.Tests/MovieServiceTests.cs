using Moq;
using MoviesArchive.Data.Interfaces;
using MoviesArchive.Data.Models;
using MoviesArchive.Logic.Parsers;
using MoviesArchive.Logic.Services;

namespace MoviesArchive.Tests;

public class MovieServiceTests
{
    private readonly Mock<IMovieRepository> _movieRepositoryMock;
    private readonly Mock<IGenreRepository> _genreRepositoryMock;
    private readonly Mock<IFileParser> _fileParserMock;

    public MovieServiceTests()
    {
        _movieRepositoryMock = new Mock<IMovieRepository>();
        _genreRepositoryMock = new Mock<IGenreRepository>();
        _fileParserMock = new Mock<IFileParser>();
    }

    [Fact]
    public async Task GetMovieEditDtoNormalInputTest()
    {
        // Arrange
        var id = 1;
        var movieReturn = GetMovieReturn();
        var genresListReturn = GetGenresListReturn();
        _movieRepositoryMock.Setup(repo => repo.GetMovie(id)).ReturnsAsync(movieReturn);
        _genreRepositoryMock.Setup(repo => repo.GetGenresList()).ReturnsAsync(genresListReturn);

        var service = new MovieService(_movieRepositoryMock.Object, _genreRepositoryMock.Object, _fileParserMock.Object);
        var resultGenres = genresListReturn.Where(g => g.Name != "undefined").OrderBy(g => g.Name);

        // Act
        var result = await service.GetMovieEditDto(id);

        // Assert
        Assert.Equal(movieReturn.Title, result.Title);
        Assert.Equal(resultGenres, result.Genres);
    }

    [Fact]
    public async Task GetMovieEditDtoWrongIdTest()
    {
        // Arrange
        var id = -1;
        var genresListReturn = GetGenresListReturn();
        _genreRepositoryMock.Setup(repo => repo.GetGenresList()).ReturnsAsync(genresListReturn);
        var service = new MovieService(_movieRepositoryMock.Object, _genreRepositoryMock.Object, _fileParserMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => service.GetMovieEditDto(id));
    }

    [Fact]
    public async Task GetMovieEditDtoIdIsCharTest()
    {
        // Arrange
        var id = '1';
        var genresListReturn = GetGenresListReturn();
        _genreRepositoryMock.Setup(repo => repo.GetGenresList()).ReturnsAsync(genresListReturn);
        var service = new MovieService(_movieRepositoryMock.Object, _genreRepositoryMock.Object, _fileParserMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => service.GetMovieEditDto(id));
    }

    private List<Genre> GetGenresListReturn()
    {
        var genres = new List<Genre>
        {
            new Genre { Id = 1, Name = "undefined" },
            new Genre { Id = 2, Name = "Comedy" },
            new Genre { Id = 3, Name = "Horror" },
            new Genre { Id = 4, Name = "Drama" },
            new Genre { Id = 5, Name = "Western" },
            new Genre { Id = 6, Name = "Science Fiction" },
        };
        return genres;
    }

    private Movie GetMovieReturn()
    {
        var movie = new Movie
        {
            Id = 1,
            Title = "Bruce Almighty",
            Rating = 8,
            ReleaseYear = 2003,
            Comment = "A great movie",
            GenreId = 2,
        };
        return movie;
    }
}
