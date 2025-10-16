using MoviesArchive.Data.Models;

namespace MoviesArchive.Logic.Parsers;

public interface IFileParser
{
    List<Movie> ParseFile(string filePath);
}
