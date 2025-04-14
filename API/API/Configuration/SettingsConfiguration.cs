namespace API.Configuration;

using Domain.Shared.Configurations;

using Microsoft.Extensions.Options;

public static class SettingsConfiguration
{
    public static IConfigurationRoot AddConfiguration(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath($"{Directory.GetCurrentDirectory()}/Properties");            
        
        var isDevelopment = environment == Environments.Development;
        if (isDevelopment)
        {
            configurationBuilder.AddJsonFile($"appsettings.Development.json", false, true);
            configurationBuilder.AddUserSecrets<Program>();
        }
        else
        {
            configurationBuilder.AddJsonFile($"appsettings.json", false, true);
        }

        configurationBuilder.AddCommandLine(args);
        configurationBuilder.AddEnvironmentVariables();
        return configurationBuilder.Build();
    }

    public static void AddAppConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CloudinarySettings>(configuration.GetSection(nameof(CloudinarySettings)));
        services.Configure<ConnectionSettings>(configuration.GetSection(nameof(ConnectionSettings)));
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        services.Configure<StripeSettings>(configuration.GetSection(nameof(StripeSettings)));

        services.AddSingleton<IOptionsMonitor<CloudinarySettings>, OptionsMonitor<CloudinarySettings>>();
        services.AddSingleton<IOptionsMonitor<ConnectionSettings>, OptionsMonitor<ConnectionSettings>>();
        services.AddSingleton<IOptionsMonitor<JwtSettings>, OptionsMonitor<JwtSettings>>();
        services.AddSingleton<IOptionsMonitor<StripeSettings>, OptionsMonitor<StripeSettings>>();

    }

}
