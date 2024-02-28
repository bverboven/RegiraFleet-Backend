using Regira.Entities.Models.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Entities.Suppliers.ContactData;

public class Address : IHasTitle, IHasNormalizedContent
{
    [MaxLength(256)]
    public string? Title { get; set; }
    [MaxLength(256)]
    public string? Street { get; set; }
    [MaxLength(8)]
    public string? StreetNumber { get; set; }
    [MaxLength(8)]
    public string? BoxNumber { get; set; }
    [MaxLength(16)]
    public string? PoBox { get; set; }
    [MaxLength(32)]
    public string? PostalCode { get; set; }
    [MaxLength(256)]
    public string? Municipality { get; set; }
    [MaxLength(2)]
    public string? CountryCode { get; set; }
    [MaxLength(512)]
    public string? Description { get; set; }

    [MaxLength(2048)]
    public string? NormalizedContent { get; set; }
}