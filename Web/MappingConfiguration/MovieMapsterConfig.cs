using Mapster;
using MoviesArchive.Data.Models;
using MoviesArchive.Logic.Models;
using MoviesArchive.Web.ViewModels;

namespace MoviesArchive.Web.MappingConfiguration;

public class MovieMapsterConfig : TypeAdapterConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<Movie, MovieDto>.NewConfig()
            .Map(dest => dest.Genre, src => src.Genre.Name);

        TypeAdapterConfig<MovieCreateVM, Movie>.NewConfig()
            .Map(dest => dest.Title, src => src.Title.Trim())
            .Map(dest => dest.Comment, src => string.Empty, srcCond => srcCond.Comment == null)
            .Map(dest => dest.Comment, src => src.Comment.Trim())
            .Map(dest => dest.UserId, src => MapContext.Current.Parameters["UserId"], srcCond => MapContext.Current != null);

        TypeAdapterConfig<MovieEditVM, Movie>.NewConfig()
            .Map(dest => dest.Title, src => src.Title.Trim())
            .Map(dest => dest.Comment, src => string.Empty, srcCond => srcCond.Comment == null)
            .Map(dest => dest.Comment, src => src.Comment.Trim())
            .Map(dest => dest.UserId, src => MapContext.Current.Parameters["UserId"], srcCond => MapContext.Current != null);

        TypeAdapterConfig<Movie, MovieEditVM>.NewConfig()
            .Map(dest => dest.Comment, src => string.Empty, srcCond => srcCond.Comment == null)
            .Map(dest => dest.Comment, src => src.Comment)
            .Map(dest => dest.Genres, src => MapContext.Current.Parameters["Genres"], srcCond => MapContext.Current != null);
    }
}
