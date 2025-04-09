namespace Domain.Shared.Constants;

public static class CultureInfos
{
    public const string English_US = "en-US";
    public const string Serbian = "sr";

    public static List<string> SupportedCultures =
        [
            new(English_US),
            new(Serbian)
        ];
}
