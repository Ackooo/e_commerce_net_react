namespace API.Configuration;

using System.Globalization;

public static class LocalizationConfiguration
{
    public static void AddLocalizationConfiguration(this IServiceCollection services)
    {
        services.AddLocalization(opt => opt.ResourcesPath = "Resource");
        var path = AppContext.BaseDirectory;
    }

    public static void UseLocalizationConfiguration(this WebApplication app)
    {
        var supportedCultures = new List<CultureInfo>
        {
            new CultureInfo("en-US"),
            new CultureInfo("sr")
        };

        var localizationOptions = new RequestLocalizationOptions
        {
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures,
            DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-US")
        };

        app.UseRequestLocalization(localizationOptions);
    }
}
