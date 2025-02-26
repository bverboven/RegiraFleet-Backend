using System.Globalization;
using Regira.Fleet.Core.Abstractions;

namespace Regira.Fleet.Core.Models;
public class CultureContext : ICultureContext
{
    public CultureInfo Culture { get; private set; }
    public string LangCode => Culture.TwoLetterISOLanguageName;
    public string? CountryCode => Culture.Name.Split('-').LastOrDefault();

    public CultureContext() => Culture = LoadInternal();

    private CultureInfo LoadInternal(string? culture = null)
    {
        Load(culture);
        return Culture;
    }
    public void Load(string? culture = null)
    {
        Culture = !string.IsNullOrWhiteSpace(culture)
            ? CultureInfo.GetCultureInfo(culture)
            : CultureInfo.CurrentCulture;
    }
}