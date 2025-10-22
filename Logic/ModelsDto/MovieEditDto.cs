using MoviesArchive.Data.Models;

namespace MoviesArchive.Logic.ModelsDto;

public class MovieEditDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int? Rating { get; set; }
    public int? ReleaseYear { get; set; }
    public string? Comment { get; set; }
    public int GenreId { get; set; }
    public int UserId { get; set; }
    public List<Genre>? Genres { get; set; }
}
