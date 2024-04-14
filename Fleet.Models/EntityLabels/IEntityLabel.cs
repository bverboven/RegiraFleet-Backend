using Regira.Entities.Models.Abstractions;

namespace Regira.Fleet.Models.EntityLabels;

public interface IEntityLabel : IHasTitle, ISortable, IHasTimestamps, IHasNormalizedContent
{
    int ObjectId { get; set; }
    string Value { get; set; }
    string? LabelType { get; set; }
}