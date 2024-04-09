using Regira.Normalizing;
using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Models.InterventionOperators.ContactData;

public class OperatorContactDataInputDto
{
    public int Id { get; set; }
    [MaxLength(64)]
    public string? Title { get; set; }
    [MaxLength(256)]
    public string Value { get; set; } = null!;
    [Normalized(SourceProperty = nameof(Value))]
    public ContactDataTypes DataType { get; set; }
    [MaxLength(512)]
    public string? Description { get; set; }
    public int SortOrder { get; set; }
}