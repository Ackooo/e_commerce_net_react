namespace API.Configuration;

public static class LocalizationConfiguration
{
    public static void AddLocalizationConfiguration(this IServiceCollection services)
    {
        services.AddLocalization();
    }

    public static void UseLocalizationConfiguration(this WebApplication app)
    {
        string[] supportedCultures = ["en-US", "sr"];

        var localizationOptions = new RequestLocalizationOptions()
            .SetDefaultCulture(supportedCultures[0])
            .AddSupportedCultures(supportedCultures)
            .AddSupportedUICultures(supportedCultures);
        app.UseRequestLocalization(localizationOptions);
    }
}
