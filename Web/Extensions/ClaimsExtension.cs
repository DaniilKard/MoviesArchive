using System.Security.Claims;

namespace MoviesArchive.Web.Extensions;

public static class ClaimsExtension
{
    public static int GetUserId(this IEnumerable<Claim> claims)
    {
        var userId = claims.First(c => c.Type == "Id").Value;
        var userIdNum = int.Parse(userId);
        return userIdNum;
    }
}
