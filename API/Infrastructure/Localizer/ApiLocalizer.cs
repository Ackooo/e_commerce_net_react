namespace Infrastructure.Localizer;

using Domain.Interfaces.Extensions;
using Localization.Resources;
using Microsoft.Extensions.Localization;

public class ApiLocalizer(IStringLocalizer<Resource> localizer) : IApiLocalizer
{

    #region ResourceManager
    /*
    public static string GetLocalizedString(string key)
    {
        if(string.IsNullOrWhiteSpace(key)) return string.Empty;
        var result = Resource.ResourceManager.GetString(key);
        return string.IsNullOrWhiteSpace(result)
            ? key
            : result;
    }

    public static string GetLocalizedString(string key, string value)
    {
        if(string.IsNullOrWhiteSpace(key)) return string.Empty;
        //return localizer[key];
        var result = Resource.ResourceManager.GetString(key);
        return string.IsNullOrWhiteSpace(result)
            ? key
            : string.Format(result, value);
    }
    */
    #endregion

    public string Translate(string key)
    {
        var result = localizer[key];
        return string.IsNullOrWhiteSpace(result)
            ? key
            : result;
    }

    public string Translate(string key, string value)
    {
        var result = localizer[key, value];
        return string.IsNullOrWhiteSpace(result)
            ? key
            : result;
    }

}
