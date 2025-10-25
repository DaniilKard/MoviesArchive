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
    private readonly IMovieService _movieService;
    private readonly IGenreService _genreService;

    public MovieController(IMovieService service, IGenreService genreService)
    {
        _movieService = service;
        _genreService = genreService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(MovieSort sort = MovieSort.TitleAsc, int page = 1)
    {
        var userId = User.Claims.First(c => c.Type == "Id").Value;
        var userIdNum = int.Parse(userId);
        var movieIndexDto = User.IsInRole("Admin") ?
            await _movieService.GetIndexPageMovies(sort, page) :
            await _movieService.GetIndexPageMovies(sort, page, userIdNum);
        var movieVM = movieIndexDto.Adapt<MovieIndexVM>();
        return View(movieVM);
    }

    [HttpGet]
    public async Task<ActionResult> AddFileToDatabase()
    {
        var userId = User.Claims.First(c => c.Type == "Id").Value;
        var userIdNum = int.Parse(userId);
        var result = await _movieService.AddFileToDatabase(userIdNum);
        return result switch
        {
            ResultStatus.Success => Ok(),
            ResultStatus.NotFound => NotFound(),
            _ => BadRequest()
        };
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
        var userId = User.Claims.First(c => c.Type == "Id").Value;
        var userIdNum = int.Parse(userId);
        var movie = model.Adapt<Movie>();
        movie.UserId = userIdNum;
        var result = await _movieService.AddMovie(movie);
        TempData["result"] = result == ResultStatus.Success ?
            $"Movie \"{movie.Title}\" was added successfully" : 
            "Movie wasn't added. Are you sure movie with this name doesn't already exist?";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> EditMovie(int? id)
    {
        if (id is not null)
        {
            var userId = User.Claims.First(c => c.Type == "Id").Value;
            var userIdNum = int.Parse(userId);
            var movie = await _movieService.GetMovieEditDto((int)id);
            if (movie is not null && (movie.UserId == userIdNum || User.IsInRole("Admin")))
            {
                var movieVM = movie.Adapt<MovieEditVM>(); //Error
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
        var userId = User.Claims.First(c => c.Type == "Id").Value;
        var userIdNum = int.Parse(userId);
        var movie = model.BuildAdapter().AddParameters("UserId", userIdNum).AdaptToType<Movie>();
        var result = await _movieService.UpdateMovie(movie);
        TempData["result"] = result == ResultStatus.Success ?
            $"Movie was edited successfully" : "An error was encountered during movie edit operation";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> DeleteMovie(int? id)
    {
        if (id is not null)
        {
            var userId = User.Claims.First(c => c.Type == "Id").Value;
            var userIdNum = int.Parse(userId);
            var movie = await _movieService.GetMovie((int)id);
            if (movie is not null && (movie.UserId == userIdNum || User.IsInRole("Admin")))
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
                $"Movie was deleted successfully" : "Movie was not deleted";
        }
        return RedirectToAction(nameof(Index));
    }
}