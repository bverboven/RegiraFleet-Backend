using Regira.Entities.Web.Attachments.Models;
using Regira.Fleet.Entities.InterventionTypes;
using Regira.Fleet.Entities.InterventionOperators.ContactData;
using Regira.Fleet.Entities.Addresses;

namespace Regira.Fleet.Entities.InterventionOperators.Operators;

public class OperatorDto
{
    public int Id { get; set; }
    public string Guid { get; set; } = null!;
    public int ClientId { get; set; }
    public string? Code { get; set; }
    public string? Title { get; set; }

    public string? IdentificationNumber { get; set; }

    public string? Description { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
    public bool IsArchived { get; set; }

    public ICollection<AddressDto>? Addresses { get; set; }
    public ICollection<OperatorContactDataDto>? ContactData { get; set; }
    public ICollection<InterventionTypeDto>? InterventionTypes { get; set; }
    public ICollection<EntityAttachmentDto>? Attachments { get; set; }
}