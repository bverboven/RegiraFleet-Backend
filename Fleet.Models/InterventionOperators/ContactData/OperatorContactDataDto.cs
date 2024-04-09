namespace Regira.Fleet.Models.InterventionOperators.ContactData;

public class OperatorContactDataDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string Value { get; set; } = null!;
    public ContactDataTypes DataType { get; set; }
    public string? Description { get; set; }
    public int SortOrder { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
}