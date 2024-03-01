using Regira.Entities.Models.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Entities.InterventionOperators.Addresses;

public class Address : IEntityWithSerial, IHasTitle, IHasNormalizedContent, ISortable
{
    public int Id { get; set; }
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
    [MaxLength(16)]
    public string? PostalCode { get; set; }
    [MaxLength(256)]
    public string? Municipality { get; set; }
    [MaxLength(2)]
    public string? CountryCode { get; set; }
    [MaxLength(512)]
    public string? Description { get; set; }
    public int SortOrder { get; set; }

    [MaxLength(2048)]
    public string? NormalizedContent { get; set; }
}