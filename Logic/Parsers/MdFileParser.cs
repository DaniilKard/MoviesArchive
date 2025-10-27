namespace MoviesArchive.Logic.Parsers;

internal class MdFileParser : FileParser
{
    public override List<string> ReadFileLines(string filePath)
    {
        var fileLines = new List<string>();
        using var streamReader = new StreamReader(filePath);
        string? line;
        while ((line = streamReader.ReadLine()) is not null)
        {
            if (line != string.Empty)
            {
                fileLines.Add(line);
            }
        }
        return fileLines;
    }
}
