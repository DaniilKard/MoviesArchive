using Microsoft.AspNetCore.Mvc;
using MoviesArchive.Web.Controllers;
using System.ComponentModel.DataAnnotations;

namespace MoviesArchive.Web.ViewModels;

public class UserRegisterVM
{
    [Remote(nameof(UserController.VerifyNameNotUsed), "User", ErrorMessage = "Similar name is already used")]
    public string Name { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }


    [Compare("Password", ErrorMessage = "Passwords don't match")]
    public string PasswordConfirm { get; set; }
}
