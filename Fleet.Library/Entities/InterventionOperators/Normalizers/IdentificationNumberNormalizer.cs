using Regira.Fleet.Utilities;

namespace Regira.Fleet.Entities.InterventionOperators.Normalizers;

public class IdentificationNumberNormalizer
{
    public string? Normalize(string? input, string? countryCode = null, bool applyPadding = true)
        => IdentificationNumberUtility.Normalize(input, countryCode, applyPadding);
}