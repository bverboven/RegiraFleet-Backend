using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Api.Models.Input;

public class SupplierInputDto
{
    public int? SupplierTypeId { get; set; }
    [MaxLength(10)]
    public string? Code { get; set; }
    [Required]
    [MaxLength(128)]
    public string? Name { get; set; }
    [MaxLength(128)]
    public string? Name2 { get; set; }
    [MaxLength(128)]
    public string? Address1 { get; set; }
    [MaxLength(128)]
    public string? Address2 { get; set; }
    [MaxLength(2)]
    public string? CountryCode { get; set; }
    [MaxLength(32)]
    public string? PostalCode { get; set; }
    [MaxLength(128)]
    public string? Location { get; set; }
    [MaxLength(64)]
    public string? Phone1 { get; set; }
    [MaxLength(64)]
    public string? Phone2 { get; set; }
    [MaxLength(32)]
    public string? VATNumber { get; set; }
    public string? Notes { get; set; }
    public bool IsArchived { get; set; }
}