using FourTails.Api.Configurations.Mappings;

namespace FourTails.Api.Configurations;

public static class MappersConfiguration
{
    public static void AddMapConfiguration(this IServiceCollection service)
    {
        service.AddAutoMapper(typeof(UserProfile).Assembly);
    }
}