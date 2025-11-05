using Serilog.Context;

namespace MoviesArchive.Web;

internal class LogUserNameMiddleware
{
    private readonly RequestDelegate next;

    public LogUserNameMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public Task Invoke(HttpContext context)
    {
        LogContext.PushProperty("UserName", context.User.Identity.Name);
        return next(context);
    }
}