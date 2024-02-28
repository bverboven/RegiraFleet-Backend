namespace Regira.Fleet.Identity.Abstractions;

public interface ISiteContext
{
    string Name { get; set; }
    bool DisableClaims { get; set; }
}