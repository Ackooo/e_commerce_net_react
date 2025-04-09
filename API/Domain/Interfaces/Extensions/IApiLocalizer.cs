namespace Domain.Interfaces.Extensions;

public interface IApiLocalizer
{
    string Translate(string key);
    string Translate(string key, string value);

}
