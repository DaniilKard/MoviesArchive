using MoviesArchive.Data.Enums;
using MoviesArchive.Logic.Models;

namespace MoviesArchive.Web.ViewModels;

public class MovieIndexVM
{
    public List<MovieDto>? Movies { get; set; }
    public MovieSort Sort { get; set; }
    public int CurrentPage { get;set; }
    public int PagesCount { get; set; }
}
