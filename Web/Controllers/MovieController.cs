using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviesArchive.Data.Enums;
using MoviesArchive.Data.Models;
using MoviesArchive.Logic.IServices;
using MoviesArchive.Web.ViewModels;

namespace MoviesArchive.Web.Controllers;

[Authorize]
public class MovieController : Controller
{
    private readonly IConfiguration _config;
    private readonly IMovieService _movieService;
    private readonly IGenreService _genreService;

    public MovieController(IMovieService service, IGenreService genreService, IConfiguration config)
    {
        _config = config;
        _movieService = service;
        _genreService = genreService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(MovieSort sort = MovieSort.TitleAsc, int page = 1)
    {
        var elementsOnPage = _config.GetValue<int>("ElementsOnOnePage");
        var userIsAdmin = User.IsInRole("Admin");
        var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
        var pagesCount = userIsAdmin ? 
            await _movieService.GetPageCount(elementsOnPage) : 
            await _movieService.GetPageCount(elementsOnPage, userId);
        var movies = userIsAdmin ?
            await _movieService.GetSortedMovies(sort, page, elementsOnPage) :
            await _movieService.GetSortedMovies(sort, page, elementsOnPage, userId);
        var movieVM = new MovieIndexVM
        {
            Sort = sort,
            CurrentPage = page,
            PagesCount = pagesCount,
            Movies = movies
        };
        return View(movieVM);
    }

    [HttpGet]
    public async Task<ActionResult> AddFileToDatabase()
    {
        var filePath = _config.GetValue<string>("MoviesFilePath");
        if (filePath is not null)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
            var result = await _movieService.AddFileToDatabase(filePath, userId);
            if (result == ResultStatus.Success)
            {
                return Ok();
            }
        }
        return BadRequest();
    }

    [HttpGet]
    public async Task<IActionResult> CreateMovie()
    {
        var genres = await _genreService.GetGenresList();
        var movieCreateVM = new MovieCreateVM
        {
            Genres = genres
        };
        return View(movieCreateVM);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMovie(MovieCreateVM model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
        var newMovie = model.Adapt<Movie>();
        newMovie.UserId = userId;
        var result = await _movieService.AddMovie(newMovie);
        TempData["result"] = result == ResultStatus.Success ?
            "Movie was added successfully" : 
            "Movie wasn't added. Are you sure movie with this name doesn't already exist?";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> EditMovie(int? id)
    {
        if (id is not null)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
            var movie = await _movieService.GetMovie((int)id);
            if (movie is not null && (movie.UserId == userId || User.IsInRole("Admin")))
            {
                var genres = await _genreService.GetGenresList();
                //var movieVM = movie.BuildAdapter().AddParameters("Genre", genres).AdaptToType<MovieEditVM>();
                var movieVM = movie.Adapt<MovieEditVM>();
                movieVM.Genres = genres;
                return View(movieVM);
            }
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> EditMovie(MovieEditVM model)
    {

        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
        var movie = model.BuildAdapter().AddParameters("UserId", userId).AdaptToType<Movie>();
        var result = await _movieService.UpdateMovie(movie);
        TempData["result"] = result == ResultStatus.Success ?
            $"Movie \"{movie.Title}\" was edited successfully" : "An error was encountered during movie edit operation";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> DeleteMovie(int? id)
    {
        if (id is not null)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
            var movie = await _movieService.GetMovie((int)id);
            if (movie is not null && (movie.UserId == userId || User.IsInRole("Admin")))
            {
                var movieVM = movie.Adapt<MovieDeleteVM>();
                return View(movieVM);
            }
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteMovie(MovieDeleteVM model)
    {
        if (model is not null)
        {
            var movie = model.Adapt<Movie>();
            var result = await _movieService.RemoveMovie(movie);
            TempData["result"] = result == ResultStatus.Success ? 
                "Movie was deleted successfully" : "Movie was not deleted";
        }
        return RedirectToAction(nameof(Index));
    }
}