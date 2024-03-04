using Regira.Entities.Models.Abstractions;
using Regira.Normalizing;
using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Entities.InterventionOperators.ContactData;

public class InterventionOperatorContactData : IEntityWithSerial, ISortable
{
    public int Id { get; set; }
    [MaxLength(64)]
    public string? Title { get; set; }
    [MaxLength(256)]
    public string Value { get; set; } = null!;
    [Normalized(SourceProperty = nameof(Value))]
    [MaxLength(256)]
    public string? NormalizedValue { get; set; }
    public ContactDataTypes DataType { get; set; }
    [MaxLength(512)]
    public string? Description { get; set; }
    public int SortOrder { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime? LastModified { get; set; }
}