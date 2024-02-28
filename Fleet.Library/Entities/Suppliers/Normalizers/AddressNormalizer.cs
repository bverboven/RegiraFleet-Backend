using Regira.Fleet.Entities.Suppliers.ContactData;
using Regira.Globalization;
using Regira.Normalizing.Abstractions;

namespace Regira.Fleet.Entities.Suppliers.Normalizers;

public class AddressNormalizer(INormalizer normalizer)
{
    private const string DEFAULT_LANGCODE = "nl";

    public void Normalize(Address? address, string? langCode = null)
    {
        if (address == null)
        {
            return;
        }

        address.NormalizedContent = Normalize(new[] { address.CountryCode, address.PostalCode, address.Municipality, address.PoBox, address.Street, address.StreetNumber, address.BoxNumber }, langCode);
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