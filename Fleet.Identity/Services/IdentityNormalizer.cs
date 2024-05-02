using Regira.Normalizing.Abstractions;

namespace Regira.Fleet.Identity.Services;

public class IdentityNormalizer : INormalizer
{
    public string? Normalize(string? input)
        => input?.Normalize()?.ToUpperInvariant();
}
