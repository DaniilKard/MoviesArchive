namespace MoviesArchive.Logic.Models;

public class MovieDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int? Rating { get; set; }
    public int? ReleaseYear { get; set; }
    public string Comment { get; set; }
    public int GenreId { get; set; }
    public int UserId { get; set; }
    public string? Genre { get; set; }
}
