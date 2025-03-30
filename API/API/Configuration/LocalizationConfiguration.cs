namespace API.Configuration;

using System.Globalization;

using Domain.Shared.Constants;

using Microsoft.AspNetCore.Localization;

public static class LocalizationConfiguration
{
    public static void AddLocalizationConfiguration(this IServiceCollection services)
    {
        services.AddLocalization(opt => opt.ResourcesPath = "Resource");
        //var path = AppContext.BaseDirectory;
    }

    public static void UseLocalizationConfiguration(this WebApplication app)
    {
		var supportedCultures = new List<CultureInfo>
		{
			new(CultureInfos.English_US),
			new(CultureInfos.Serbian)
		};

        var localizationOptions = new RequestLocalizationOptions
        {
            SupportedCultures = supportedCultures,
            SupportedUICultures = supportedCultures,
            DefaultRequestCulture = new RequestCulture(CultureInfos.English_US)
        };

        app.UseRequestLocalization(localizationOptions);
    }
}
