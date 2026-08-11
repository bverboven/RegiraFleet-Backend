using Regira.Entities.Attachments.Models;

namespace Regira.Fleet.Models.InterventionOperators.Operators;

public class OperatorAttachment : EntityAttachment
{
    // Operator is IArchivable, and the mirrored query filter on this link needs a navigation to bind to.
    public Operator? Operator { get; set; }
}
