using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Localization.Config;

public static class LocalizationDependencyRegistry
{
    public static IServiceCollection AddLocalizationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddLocalization();

        return services;
    }
}