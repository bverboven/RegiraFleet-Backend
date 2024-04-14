namespace Regira.Fleet.Models.EntityLabels;

public interface IHasLabels
{
    ICollection<IEntityLabel>? Labels { get; set; }
}
public interface IHasLabels<T>
    where T : IEntityLabel
{
    ICollection<T>? Labels { get; set; }
}