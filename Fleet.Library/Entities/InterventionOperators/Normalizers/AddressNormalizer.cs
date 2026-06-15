using Regira.Fleet.Models.Addresses;
using Regira.Globalization.Utilities;
using Regira.Normalizing.Abstractions;

namespace Regira.Fleet.Entities.InterventionOperators.Normalizers;

public class AddressNormalizer(INormalizer normalizer)
{
    private const string DEFAULT_LANGCODE = "nl";

    public void Normalize(Address? address, string? langCode = null)
    {
        if (address == null)
        {
            return;
        }

        address.NormalizedContent = Normalize([address.CountryCode, address.PostalCode, address.City, address.PostBox, address.Street, address.Number, address.Box
        ], langCode);
    }

    public string? Normalize(string?[] addressSegments, string? langCode = null)
    {
        if (!addressSegments.Any() || addressSegments.All(string.IsNullOrWhiteSpace))
        {
            return null;
        }

        var country = CountryUtility.GetCountry(addressSegments.First());
        var countryName = country?.GetName(langCode ?? DEFAULT_LANGCODE) ?? country?.Title;
        var normalizedAddress = string.Join(' ', new[] { countryName }
                .Concat(addressSegments.Skip(1))
                .Where(x => !string.IsNullOrWhiteSpace(x))
            ).Trim();
        if (string.IsNullOrWhiteSpace(normalizedAddress))
        {
            normalizedAddress = null;
        }
        return normalizer.Normalize(normalizedAddress);
    }
}