namespace MoviesArchive.Web.ViewModels;

public class UserLoginVM
{
    //[Remote(nameof(UserController.VerifyNameUsed), "User", ErrorMessage = "No user found. Are you sure name is correct?")]
    public string Name { get; set; }
    public string Password { get; set; }
}
