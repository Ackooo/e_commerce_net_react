namespace API.Configuration;

using Domain.Mappings;

public static class MappingConfiguration
{
    public static void AddMappingConfiguration(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfiles).Assembly);
    }

}
