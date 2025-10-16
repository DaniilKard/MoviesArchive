using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using MoviesArchive.Data.Models;

namespace MoviesArchive.Logic.Parsers;

internal class DocxFileParser : IFileParser
{
    public List<Movie> ParseFile(string filePath)
    {
        var fileLines = new List<string>();
        using (var wordDoc = WordprocessingDocument.Open(filePath, false))
        {
            var body = wordDoc.MainDocumentPart.Document.Body;
            foreach (var paragraph in body.Elements<Paragraph>())
            {
                if (paragraph.InnerText != string.Empty)
                {
                    fileLines.Add(paragraph.InnerText);
                }
            }
        }

        var movies = new List<Movie>();
        var currentGenre = new Genre
        {
            Name = "undefined"
        };
        foreach (var line in fileLines)
        {
            // Get genre
            if (line.StartsWith("#"))
            {
                currentGenre = new Genre
                {
                    Name = line.Substring(2)
                };
                continue;
            }
            var movie = new Movie
            {
                Title = line,
                Genre = currentGenre
            };
            // Get rating
            if (float.TryParse(line.AsSpan(0, 3), out var outFloatRating))
            {
                movie.Rating = (int)(outFloatRating * 10);
            }
            else if (int.TryParse(line.AsSpan(0, 2), out var outIntRating))
            {
                movie.Rating = outIntRating * 10;
            }

            if (line.Contains('('))
            {
                // Get release year
                if (line.IndexOf(')') - line.IndexOf('(') > 4)
                {
                    if (int.TryParse(line.AsSpan(line.IndexOf('(') + 1, 4), out var outReleaseYear))
                    {
                        movie.ReleaseYear = outReleaseYear;
                    }
                }
                // Get comment
                if (movie.ReleaseYear is null)
                {
                    movie.Comment = line.Substring(line.IndexOf('(') + 1, line.IndexOf(')') - line.IndexOf('(') - 1);
                }
                else if (line.IndexOf(')') - line.IndexOf('(') > 5)
                {
                    movie.Comment = line.Substring(line.IndexOf('(') + 6, line.Length - line.IndexOf('(') - 7);
                }
            }
            // Get title
            if (movie.Rating is not null)
            {
                movie.Title = movie.Title.Substring(movie.Title.IndexOf(' ') + 1);
            }
            if (line.Contains('('))
            {
                movie.Title = movie.Title.Substring(0, movie.Title.IndexOf('(') - 1);
            }
            movies.Add(movie);
        }
        return movies;
    }
}
