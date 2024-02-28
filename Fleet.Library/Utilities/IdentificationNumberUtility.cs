using System.Text.RegularExpressions;

namespace Regira.Fleet.Utilities;

public static class IdentificationNumberUtility
{
    public static string? Normalize(string? input, string? countryCode = null, bool applyPadding = true)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return null;
        }

        if (input.StartsWith("BE") || countryCode == "BE")
        {
            var numericIdentificationNumber = Regex.Replace(input, "[^0-9]", "");
            var paddedIdentificationNumber = numericIdentificationNumber.PadLeft(10, '0');
            var output = $"BE{(applyPadding ? paddedIdentificationNumber : numericIdentificationNumber)}";
            return output;
        }

        return input;
    }

    public static string? Format(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return input;
        }

        var normalizedInput = Normalize(input);
        if (normalizedInput?.StartsWith("BE") == true)
        {
            var numericInput = normalizedInput.Substring(2);
            var output = $"BE {numericInput.Substring(0, 4)}.{numericInput.Substring(4, 3)}.{numericInput.Substring(7)}";
            return output;
        }

        return input;
    }
}