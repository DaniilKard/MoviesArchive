using Autofac;
using Microsoft.AspNetCore.Identity;
using MoviesArchive.Data.Interfaces;
using MoviesArchive.Data.Models;
using MoviesArchive.Logic.Authorization;
using MoviesArchive.Logic.IServices;
using MoviesArchive.Logic.Parsers;

namespace MoviesArchive.Web;

public class AutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        var dataAssembly = typeof(IMovieRepository).Assembly;
        var logicAssembly = typeof(IMovieService).Assembly;

        builder.RegisterType<AuthorizeWithCookies>().As<IUserAuthorize>();
        builder.RegisterType<PasswordHasher<User>>().As<IPasswordHasher<User>>();

        builder.RegisterAssemblyTypes(logicAssembly)
            .Where(t => t.Name.EndsWith("Parser"))
            .As<IFileParser>();

        builder.RegisterAssemblyTypes(logicAssembly)
            .Where(t => t.Name.EndsWith("Service"))
            .AsImplementedInterfaces();

        builder.RegisterAssemblyTypes(dataAssembly)
            .Where(t => t.Name.EndsWith("Repository"))
            .AsImplementedInterfaces();
    }
}
