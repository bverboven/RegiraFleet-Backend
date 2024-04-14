namespace Regira.Fleet.Models.EntityLabels;

public class EntityLabelDto
{
    public int Id { get; set; }
    public int ObjectId { get; set; }
    public string? Title { get; set; }
    public string Value { get; set; } = null!;
    public string? LabelType { get; set; }
    public int SortOrder { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
}
