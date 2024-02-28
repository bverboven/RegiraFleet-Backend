using Regira.Globalization;
using System.Globalization;

namespace Regira.Fleet.Culture;

public class CultureContext
{
    public CultureInfo Culture { get; }
    public string? LangCode { get; }
    public string? CountryCode { get; }
    public Country? HomeCountry { get; }
    public CultureContext(string? culture = null)
    {
        Culture = !string.IsNullOrWhiteSpace(culture)
            ? CultureInfo.GetCultureInfo(culture)
            : CultureInfo.CurrentCulture;
        LangCode = Culture.TwoLetterISOLanguageName;
        CountryCode = Culture.Name.Split('-').LastOrDefault();
        HomeCountry = CountryUtility.GetCountry(CountryCode);
    }
}