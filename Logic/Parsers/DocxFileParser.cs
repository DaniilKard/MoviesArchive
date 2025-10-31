using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace MoviesArchive.Logic.Parsers;

internal class DocxFileParser : FileParser
{
    private protected override List<string> ReadFileLines(string filePath)
    {
        var fileLines = new List<string>();
        using var wordDoc = WordprocessingDocument.Open(filePath, false);
        var body = wordDoc.MainDocumentPart.Document.Body;
        foreach (var paragraph in body.Elements<Paragraph>())
        {
            if (paragraph.InnerText != string.Empty)
            {
                fileLines.Add(paragraph.InnerText);
            }
        }
        return fileLines;
    }
}
