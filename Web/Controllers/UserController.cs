using Mapster;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviesArchive.Data.Enums;
using MoviesArchive.Logic.IServices;
using MoviesArchive.Web.ViewModels;

namespace MoviesArchive.Web.Controllers;

public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UserIndex()
    {
        var users = await _userService.GetUsersList();
        var usersVM = users.Select(u => u.Adapt<UserIndexVM>()).ToList();
        return View(usersVM);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(UserLoginVM model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var userIp = HttpContext.Connection.RemoteIpAddress.ToString();
        var result = await _userService.LoginUser(model.Name, model.Password, userIp);
        if (result != ResultStatus.Success)
        {
            ModelState.AddModelError("Password", "Password is incorrect");
            return View(model);
        }
        return RedirectToAction(nameof(MovieController.Index), "Movie");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(UserRegisterVM model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var userIp = HttpContext.Connection.RemoteIpAddress.ToString();
        var result = await _userService.RegisterUser(model.Name, model.Email, model.Password, userIp);
        if (result != ResultStatus.Success)
        {
            ModelState.AddModelError("Email", "Email is already taken");
            return View(model);
        }
        return RedirectToAction(nameof(MovieController.Index), "Movie");
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }

    [AcceptVerbs("GET", "POST")]
    public async Task<IActionResult> VerifyNameNotUsed(string? name)
    {
        var nameIsUsed = string.IsNullOrWhiteSpace(name) ? false : await _userService.UserNameExists(name);
        return Json(!nameIsUsed);
    }

    [AcceptVerbs("GET", "POST")]
    public async Task<IActionResult> VerifyNameUsed(string? name)
    {
        var nameIsUsed = string.IsNullOrWhiteSpace(name) ? true : await _userService.UserNameExists(name);
        return Json(nameIsUsed);
    }
}
