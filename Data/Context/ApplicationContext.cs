using Microsoft.EntityFrameworkCore;
using MoviesArchive.Data.Models;

namespace MoviesArchive.Data.Context;

public class ApplicationContext : DbContext
{
    public DbSet<Movie> Movies { get; set; } = null!;
    public DbSet<Genre> Genres { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<IpAddress> IpAddresses { get; set; } = null!;
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) 
    {
        
    }
}
