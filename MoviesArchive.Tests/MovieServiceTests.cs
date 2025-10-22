using Microsoft.Extensions.Configuration;
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
    public async Task AddFileToDatabaseNormalTest()
    {
        var iMovieRepoMock = new Mock<IMovieRepository>();
        var iGenreRepoMock = new Mock<IGenreRepository>();
        var iFileParserMock = new Mock<IFileParser>();
        var customConfigValue = new Dictionary<string, string>
        {
            {"MoviesFilePath", "D:\\visual_projects\\C#\\MoviesArchive movies files"}
        };
        var configMock = new ConfigurationBuilder().AddInMemoryCollection(customConfigValue).Build();

        iFileParserMock.Setup(par => par.ParseFile("")).Returns([new(), new()]);
        iMovieRepoMock.Setup(repo => repo.GetMovieTitlesForUser(1)).ReturnsAsync(["Movie1", "Movie2"]);
        iGenreRepoMock.Setup(repo => repo.GetGenresList()).ReturnsAsync([new Genre(), new Genre()]);
        iMovieRepoMock.Setup(repo => repo.AddMovieRange(null)).ReturnsAsync(1);

        var service = new MovieService(iMovieRepoMock.Object, iGenreRepoMock.Object, configMock, iFileParserMock.Object);
        var result = await service.AddFileToDatabase(1);

        Assert.Equal(ResultStatus.Success, result);
    }

    [Fact]
    public async Task AddFileToDatabaseFilepathNullTest()
    {
        var iMovieRepoMock = new Mock<IMovieRepository>();
        var iGenreRepoMock = new Mock<IGenreRepository>();
        var iFileParserMock = new Mock<IFileParser>();
        var customConfigValue = new Dictionary<string, string>
        {
            {"MoviesFilePath", null}
        };
        var configMock = new ConfigurationBuilder().AddInMemoryCollection(customConfigValue).Build();

        iFileParserMock.Setup(par => par.ParseFile("")).Returns([new(), new()]);
        iMovieRepoMock.Setup(repo => repo.GetMovieTitlesForUser(1)).ReturnsAsync(["Movie1", "Movie2"]);
        iGenreRepoMock.Setup(repo => repo.GetGenresList()).ReturnsAsync([new Genre(), new Genre()]);
        iMovieRepoMock.Setup(repo => repo.AddMovieRange(null)).ReturnsAsync(1);

        var service = new MovieService(iMovieRepoMock.Object, iGenreRepoMock.Object, configMock, iFileParserMock.Object);
        var result = await service.AddFileToDatabase(1);

        Assert.Equal(ResultStatus.Failed, result);
    }

    [Fact]
    public async Task AddFileToDatabaseFilepathEmptyTest()
    {
        var iMovieRepoMock = new Mock<IMovieRepository>();
        var iGenreRepoMock = new Mock<IGenreRepository>();
        var iFileParserMock = new Mock<IFileParser>();
        var customConfigValue = new Dictionary<string, string>
        {
            {"MoviesFilePath", ""}
        };
        var configMock = new ConfigurationBuilder().AddInMemoryCollection(customConfigValue).Build();

        iFileParserMock.Setup(par => par.ParseFile("")).Returns([new(), new()]);
        iMovieRepoMock.Setup(repo => repo.GetMovieTitlesForUser(1)).ReturnsAsync(["Movie1", "Movie2"]);
        iGenreRepoMock.Setup(repo => repo.GetGenresList()).ReturnsAsync([new Genre(), new Genre()]);
        iMovieRepoMock.Setup(repo => repo.AddMovieRange(null)).ReturnsAsync(1);

        var service = new MovieService(iMovieRepoMock.Object, iGenreRepoMock.Object, configMock, iFileParserMock.Object);
        var result = await service.AddFileToDatabase(1);

        Assert.Equal(ResultStatus.Failed, result);
    }

    [Fact]
    public async Task AddFileToDatabaseUserIdNegativeTest()
    {
        var iMovieRepoMock = new Mock<IMovieRepository>();
        var iGenreRepoMock = new Mock<IGenreRepository>();
        var iFileParserMock = new Mock<IFileParser>();
        var customConfigValue = new Dictionary<string, string>
        {
            {"MoviesFilePath", "D:\\visual_projects\\C#\\MoviesArchive movies files"}
        };
        var configMock = new ConfigurationBuilder().AddInMemoryCollection(customConfigValue).Build();

        iFileParserMock.Setup(par => par.ParseFile("")).Returns([new(), new()]);
        iMovieRepoMock.Setup(repo => repo.GetMovieTitlesForUser(-1)).ReturnsAsync([]);
        iGenreRepoMock.Setup(repo => repo.GetGenresList()).ReturnsAsync([new Genre(), new Genre()]);
        iMovieRepoMock.Setup(repo => repo.AddMovieRange(null)).ReturnsAsync(1);

        var service = new MovieService(iMovieRepoMock.Object, iGenreRepoMock.Object, configMock, iFileParserMock.Object);
        var result = await service.AddFileToDatabase(-1);

        Assert.Equal(ResultStatus.Failed, result);
    }

    [Fact]
    public async Task AddFileToDatabaseFilepathRandomTest()
    {
        var iMovieRepoMock = new Mock<IMovieRepository>();
        var iGenreRepoMock = new Mock<IGenreRepository>();
        var iFileParserMock = new Mock<IFileParser>();
        var customConfigValue = new Dictionary<string, string>
        {
            {"MoviesFilePath", "aXF2Xs7foN"}
        };
        var configMock = new ConfigurationBuilder().AddInMemoryCollection(customConfigValue).Build();

        iFileParserMock.Setup(par => par.ParseFile("")).Returns([new(), new()]);
        iMovieRepoMock.Setup(repo => repo.GetMovieTitlesForUser(1)).ReturnsAsync(["Movie1", "Movie2"]);
        iGenreRepoMock.Setup(repo => repo.GetGenresList()).ReturnsAsync([new Genre(), new Genre()]);
        iMovieRepoMock.Setup(repo => repo.AddMovieRange(null)).ReturnsAsync(1);

        var service = new MovieService(iMovieRepoMock.Object, iGenreRepoMock.Object, configMock, iFileParserMock.Object);
        var result = await service.AddFileToDatabase(1);

        Assert.Equal(ResultStatus.Failed, result);
    }

    [Fact]
    public async Task AddFileToDatabaseUserIdIsWhitespaceCharTest()
    {
        var iMovieRepoMock = new Mock<IMovieRepository>();
        var iGenreRepoMock = new Mock<IGenreRepository>();
        var iFileParserMock = new Mock<IFileParser>();
        var customConfigValue = new Dictionary<string, string>
        {
            {"MoviesFilePath", "D:\\visual_projects\\C#\\MoviesArchive movies files"}
        };
        var configMock = new ConfigurationBuilder().AddInMemoryCollection(customConfigValue).Build();

        iFileParserMock.Setup(par => par.ParseFile("")).Returns([new(), new()]);
        iMovieRepoMock.Setup(repo => repo.GetMovieTitlesForUser(' ')).ReturnsAsync([]);
        iGenreRepoMock.Setup(repo => repo.GetGenresList()).ReturnsAsync([new Genre(), new Genre()]);
        iMovieRepoMock.Setup(repo => repo.AddMovieRange(null)).ReturnsAsync(1);

        var service = new MovieService(iMovieRepoMock.Object, iGenreRepoMock.Object, configMock, iFileParserMock.Object);
        var result = await service.AddFileToDatabase(' ');

        Assert.Equal(ResultStatus.Failed, result);
    }
}
