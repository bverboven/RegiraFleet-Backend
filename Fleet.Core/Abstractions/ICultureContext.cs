using System.Globalization;

namespace Regira.Fleet.Core.Abstractions;

public interface ICultureContext
{
    CultureInfo Culture { get; }
    string? LangCode { get; }
    string? CountryCode { get; }

    void Load(string? culture = null);
}
