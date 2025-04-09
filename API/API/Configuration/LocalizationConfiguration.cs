namespace API.Configuration;

using System.Globalization;
using Domain.Shared.Constants;

using Microsoft.AspNetCore.Localization;

public static class LocalizationConfiguration
{
    public static void AddLocalizationConfiguration(this IServiceCollection services)
    {
        services.AddLocalization(opt => opt.ResourcesPath = "");
    }

    public static void UseLocalizationConfiguration(this WebApplication app)
    {
        var supportedCultures = CultureInfos.SupportedCultures
            .Select(x=> new CultureInfo(x))
            .ToList();

        var localizationOptions = new RequestLocalizationOptions
        {
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures,
            DefaultRequestCulture = new RequestCulture(CultureInfos.English_US)
        };

        localizationOptions.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());

        app.UseRequestLocalization(localizationOptions);
    }
}
