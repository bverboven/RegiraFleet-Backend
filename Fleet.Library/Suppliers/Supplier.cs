using System.ComponentModel.DataAnnotations;
using Regira.Entities.Models.Abstractions;
using Regira.Fleet.SupplierTypes;

namespace Regira.Fleet.Suppliers;

public class Supplier : IEntityWithSerial, IHasCode, IArchivable
{
    public int Id { get; set; }
    public int? SupplierTypeId { get; set; }
    [MaxLength(10)]
    public string? Code { get; set; }
    [MaxLength(128)]
    public string? Name { get; set; }
    [MaxLength(128)]
    public string? Name2 { get; set; }
    [MaxLength(128)]
    public string? Address1 { get; set; }
    [MaxLength(128)]
    public string? Address2 { get; set; }
    [MaxLength(32)]
    public string? PostalCode { get; set; }
    [MaxLength(128)]
    public string? Location { get; set; }
    [MaxLength(2)]
    public string? CountryCode { get; set; }
    [MaxLength(128)]
    public string? Email { get; set; }
    [MaxLength(64)]
    public string? Phone1 { get; set; }
    [MaxLength(64)]
    public string? Phone2 { get; set; }
    [MaxLength(32)]
    public string? VATNumber { get; set; }
    public string? Notes { get; set; }
    public DateTime? Created { get; set; }
    public DateTime? LastModified { get; set; }
    public bool IsArchived { get; set; }

    public SupplierType? SupplierType { get; set; }
}