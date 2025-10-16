using Mapster;
using MoviesArchive.Data.Models;
using MoviesArchive.Web.ViewModels;

namespace MoviesArchive.Web.MappingConfiguration;

public class UserMapsterConfig : TypeAdapterConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<UserRegisterVM, User>.NewConfig()
            .Map(dest => dest.Email, src => src.Email.ToLower())
            .Map(dest => dest.Role, src => "User")
            .Map(dest => dest.RegistrationDate, src => DateOnly.FromDateTime(DateTime.Now))
            .Map(dest => dest.IpAddresses, src => MapContext.Current.Parameters["IpAddresses"], srcCond => MapContext.Current != null);
    }
}
