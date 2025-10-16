using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviesArchive.Data.Enums;
using MoviesArchive.Data.Models;
using MoviesArchive.Logic.IServices;
using MoviesArchive.Web.ViewModels;

namespace MoviesArchive.Web.Controllers;

[Authorize(Roles = "Admin")]
public class GenreController : Controller
{
    private readonly IGenreService _genreService;

    public GenreController(IGenreService genreService)
    {
        _genreService = genreService;
    }

    [HttpGet]
    public async Task<IActionResult> GenreIndex()
    {
        var genres = await _genreService.GetGenresList();
        var genresVM = genres.Select(g => g.Adapt<GenreVM>()).ToList();
        return View(genresVM);
    }

    [HttpGet]
    public IActionResult CreateGenre()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateGenre(GenreVM model)
    {        
        if (ModelState.IsValid)
        {
            var genre = model.Adapt<Genre>();
            var result = await _genreService.AddGenre(genre);
            TempData["result"] = result == ResultStatus.Success ?
                "Genre was added successfully" : "An error occurred while adding genre";
        }
        return RedirectToAction(nameof(GenreIndex));
    }

    [HttpGet]
    public async Task<IActionResult> EditGenre(int? id)
    {
        if (id is not null)
        {
            var genre = await _genreService.GetGenreById((int)id);
            if (genre is not null)
            {
                var genreVM = genre.Adapt<GenreVM>();
                return View(genreVM);
            }
        }
        return RedirectToAction(nameof(GenreIndex));
    }

    [HttpPost]
    public async Task<IActionResult> EditGenre(GenreVM model)
    {
        if (ModelState.IsValid)
        {
            var genre = model.Adapt<Genre>();
            var result = await _genreService.UpdateGenre(genre);
            TempData["result"] = result == ResultStatus.Success ? 
                "Genre was edited successfully" : "An error occurred while editing genre";
        }
        return RedirectToAction(nameof(GenreIndex));
    }

    [HttpGet]
    public async Task<IActionResult> DeleteGenre(int? id)
    {
        if (id is not null)
        {
            var genre = await _genreService.GetGenreById((int)id);
            if (genre is not null)
            {
                var genreVM = genre.Adapt<GenreVM>();
                return View(genreVM);
            }
        }
        return RedirectToAction(nameof(GenreIndex));
    }

    [HttpPost]
    public async Task<IActionResult> DeleteGenre(GenreVM model)
    {
        if (model is not null)
        {
            var genre = model.Adapt<Genre>();
            var result = await _genreService.RemoveGenre(genre);
            TempData["result"] = result == ResultStatus.Success ?
                "Genre was deleted successfully" : "An error occurred while deleting genre";
        }
        return RedirectToAction(nameof(GenreIndex));
    }
}
