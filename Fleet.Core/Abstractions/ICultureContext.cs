namespace Regira.Fleet.Core.Abstractions;

public interface ICultureContext
{
    string? LangCode { get; }
    string? CountryCode { get; }

    void Load(string? culture = null);
}
