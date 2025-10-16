using MoviesArchive.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace MoviesArchive.Web.ViewModels;

public class MovieCreateVM
{
    [Required]
    public string Title { get; set; }
    public int? Rating { get; set; }
    public int? ReleaseYear { get; set; }
    public string? Comment { get; set; }
    public int GenreId { get; set; }
    public List<Genre>? Genres { get; set; }
}