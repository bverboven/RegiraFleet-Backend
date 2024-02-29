using System.ComponentModel.DataAnnotations;
using Regira.Normalizing;

namespace Regira.Fleet.Entities.Suppliers.ContactData;

public class SupplierContactDataInputDto
{
    public int Id { get; set; }
    [MaxLength(32)]
    public string? Title { get; set; }
    [MaxLength(256)]
    public string Value { get; set; } = null!;
    [Normalized(SourceProperty = nameof(Value))]
    public ContactDataTypes DataType { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public int SortOrder { get; set; }
}