using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Entities.InterventionOperators.Addresses;

public class AddressInputDto
{
    [MaxLength(256)]
    public string? Title { get; set; }
    [MaxLength(256)]
    public string? Street { get; set; }
    [MaxLength(8)]
    public string? Number { get; set; }
    [MaxLength(8)]
    public string? PoBox { get; set; }
    [MaxLength(32)]
    public string? PostalCode { get; set; }
    [MaxLength(256)]
    public string? Municipality { get; set; }
    [MaxLength(2)]
    public string? CountryCode { get; set; }
}