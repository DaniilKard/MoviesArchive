using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using MoviesArchive.Data.Models;
using System.Security.Claims;

namespace MoviesArchive.Logic.Authorization;

public class AuthorizeWithCookies : IUserAuthorize
{
    private IHttpContextAccessor _httpContextAccessor;

    public AuthorizeWithCookies(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task Authorize(User user)
    {
        var claims = new List<Claim>
        {
            new("Id", user.Id.ToString()),
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Role, user.Role)
        };
        var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
        var HttpContext = _httpContextAccessor.HttpContext;
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
    }
}
