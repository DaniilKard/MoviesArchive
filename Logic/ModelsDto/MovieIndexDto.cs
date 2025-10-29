using MoviesArchive.Data.Enums;
using MoviesArchive.Data.Models;
using MoviesArchive.Logic.Models;

namespace MoviesArchive.Logic.ModelsDto;

public class MovieIndexDto
{
    public List<MovieDto>? Movies { get; set; }
    public MovieSort Sort { get; set; }
    public int CurrentPage { get; set; }
    public int PagesCount { get; set; }
    public List<Genre>? Genres { get; set; }
    public int SearchGenreId { get; set; }
    public string? SearchLine { get; set; }
}
