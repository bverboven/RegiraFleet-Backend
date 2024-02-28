using Regira.Entities.Models.Abstractions;
using Regira.Normalizing;
using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Entities.Suppliers.ContactData;

public class SupplierContactData : IEntityWithSerial
{
    public int Id { get; set; }
    public int SupplierId { get; set; }
    [MaxLength(32)]
    public string? Title { get; set; }
    [MaxLength(256)]
    public string Value { get; set; } = null!;
    [Normalized(SourceProperty = nameof(Value))]
    [MaxLength(256)]
    public string? NormalizedValue { get; set; }
    public ContactDataTypes DataType { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public int SortOrder { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime? LastModified { get; set; }
}