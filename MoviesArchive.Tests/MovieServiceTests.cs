using Moq;
using MoviesArchive.Data.Enums;
using MoviesArchive.Data.Interfaces;
using MoviesArchive.Data.Models;
using MoviesArchive.Logic.Parsers;
using MoviesArchive.Logic.Services;

namespace MoviesArchive.Tests;

public class MovieServiceTests
{
    [Fact]
    public async Task AddFileToDatabaseFilepathNullTest()
    {
        var iMovieRepoMock = new Mock<IMovieRepository>();
        var iGenreRepoMock = new Mock<IGenreRepository>();
        var iFileParserMock = new Mock<IFileParser>();

        iMovieRepoMock.Setup(repo => repo.GetMovieTitlesForUser(7)).ReturnsAsync(["Movie1", "Movie2"]);
        iGenreRepoMock.Setup(repo => repo.GetGenresList()).ReturnsAsync([new Genre(), new Genre()]);

        var service = new MovieService(iMovieRepoMock.Object, iGenreRepoMock.Object, iFileParserMock.Object);

        var result = await service.AddFileToDatabase(null, 1);

        Assert.Equal(ResultStatus.Failed, result);
    }

    [Fact]
    public async Task AddFileToDatabaseFilepathEmptyTest()
    {
        var iMovieRepoMock = new Mock<IMovieRepository>();
        var iGenreRepoMock = new Mock<IGenreRepository>();
        var iFileParserMock = new Mock<IFileParser>();

        iMovieRepoMock.Setup(repo => repo.GetMovieTitlesForUser(7)).ReturnsAsync(["Movie1", "Movie2"]);
        iGenreRepoMock.Setup(repo => repo.GetGenresList()).ReturnsAsync([new Genre(), new Genre()]);

        var service = new MovieService(iMovieRepoMock.Object, iGenreRepoMock.Object, iFileParserMock.Object);

        var result = await service.AddFileToDatabase("", 1);

        Assert.Equal(ResultStatus.Failed, result);
    }

    [Fact]
    public async Task AddFileToDatabaseUserIdNegativeTest()
    {
        var iMovieRepoMock = new Mock<IMovieRepository>();
        var iGenreRepoMock = new Mock<IGenreRepository>();
        var iFileParserMock = new Mock<IFileParser>();

        iMovieRepoMock.Setup(repo => repo.GetMovieTitlesForUser(7)).ReturnsAsync(["Movie1", "Movie2"]);
        iGenreRepoMock.Setup(repo => repo.GetGenresList()).ReturnsAsync([new Genre(), new Genre()]);

        var service = new MovieService(iMovieRepoMock.Object, iGenreRepoMock.Object, iFileParserMock.Object);

        var result = await service.AddFileToDatabase("C:/", -1);

        Assert.Equal(ResultStatus.Failed, result);
    }

    [Fact]
    public async Task AddFileToDatabaseUserIdZeroTest()
    {
        var iMovieRepoMock = new Mock<IMovieRepository>();
        var iGenreRepoMock = new Mock<IGenreRepository>();
        var iFileParserMock = new Mock<IFileParser>();

        iMovieRepoMock.Setup(repo => repo.GetMovieTitlesForUser(7)).ReturnsAsync(["Movie1", "Movie2"]);
        iGenreRepoMock.Setup(repo => repo.GetGenresList()).ReturnsAsync([new Genre(), new Genre()]);

        var service = new MovieService(iMovieRepoMock.Object, iGenreRepoMock.Object, iFileParserMock.Object);

        var result = await service.AddFileToDatabase("C:/", 0);

        Assert.Equal(ResultStatus.Failed, result);
    }

    [Fact]
    public async Task AddFileToDatabaseFilepathRandomTest()
    {
        var iMovieRepoMock = new Mock<IMovieRepository>();
        var iGenreRepoMock = new Mock<IGenreRepository>();
        var iFileParserMock = new Mock<IFileParser>();

        iMovieRepoMock.Setup(repo => repo.GetMovieTitlesForUser(7)).ReturnsAsync(["Movie1", "Movie2"]);
        iGenreRepoMock.Setup(repo => repo.GetGenresList()).ReturnsAsync([new Genre(), new Genre()]);

        var service = new MovieService(iMovieRepoMock.Object, iGenreRepoMock.Object, iFileParserMock.Object);

        var result = await service.AddFileToDatabase("aXF2Xs7foN", 1);

        Assert.Equal(ResultStatus.Failed, result);
    }

    [Fact]
    public async Task AddFileToDatabaseUserIdIsWhitespaceCharTest()
    {
        var iMovieRepoMock = new Mock<IMovieRepository>();
        var iGenreRepoMock = new Mock<IGenreRepository>();
        var iFileParserMock = new Mock<IFileParser>();

        iMovieRepoMock.Setup(repo => repo.GetMovieTitlesForUser(7)).ReturnsAsync(["Movie1", "Movie2"]);
        iGenreRepoMock.Setup(repo => repo.GetGenresList()).ReturnsAsync([new Genre(), new Genre()]);

        var service = new MovieService(iMovieRepoMock.Object, iGenreRepoMock.Object, iFileParserMock.Object);

        var result = await service.AddFileToDatabase("C:/", ' ');

        Assert.Equal(ResultStatus.Failed, result);
    }
}
