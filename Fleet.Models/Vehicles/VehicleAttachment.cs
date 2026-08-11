using Regira.Entities.Attachments.Models;

namespace Regira.Fleet.Models.Vehicles;

public class VehicleAttachment : EntityAttachment
{
    // Vehicle is IArchivable, and the mirrored query filter on this link needs a navigation to bind to.
    public Vehicle? Vehicle { get; set; }
}
