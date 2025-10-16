using Mapster;
using MoviesArchive.Data.Models;
using MoviesArchive.Web.ViewModels;

namespace MoviesArchive.Web.MappingConfiguration;

public class GenreMapsterConfig : TypeAdapterConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<GenreVM, Genre>.NewConfig()
            .Map(dest => dest.Name, src => string.Empty, srcCond => srcCond.Name == null)
            .Map(dest => dest.Name, src => src.Name.Trim());
    }
}
