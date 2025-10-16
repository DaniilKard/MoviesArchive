using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using MoviesArchive.Data.Context;
using MoviesArchive.Web.MappingConfiguration;
using Serilog;

namespace MoviesArchive.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connection = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
        {
            containerBuilder.RegisterModule(new AutofacModule());
        });

        Log.Logger = new LoggerConfiguration()
            .WriteTo.File("Logs.txt")
            .CreateLogger();

        // Add services to the container.
        builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlite(connection));
        builder.Services.AddMvc();
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();
        builder.Services.AddControllersWithViews();       
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
        {
            options.LoginPath = "/user/login";
        });
        builder.Services.AddHttpContextAccessor();

        MovieMapsterConfig.Configure();
        GenreMapsterConfig.Configure();
        UserMapsterConfig.Configure();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            //app.UseExceptionHandler("/Movie/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        //app.UseHttpsRedirection();
        //app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Movie}/{action=Index}/{id?}");

        app.Run();
    }
}
