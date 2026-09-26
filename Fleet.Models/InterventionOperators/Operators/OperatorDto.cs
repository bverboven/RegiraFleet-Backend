using Regira.Entities.Mapping.Models;
using Regira.Fleet.Models.Addresses;
using Regira.Fleet.Models.EntityLabels;
using Regira.Fleet.Models.InterventionOperators.ContactData;

namespace Regira.Fleet.Models.InterventionOperators.Operators;

public class OperatorDto
{
    public int Id { get; set; }
    public string Guid { get; set; } = null!;
    public string TenantId { get; set; } = null!;
    public string? Code { get; set; }
    public string? Title { get; set; }

    public string? IdentificationNumber { get; set; }

    public string? Description { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
    public bool IsArchived { get; set; }
    public Guid ConcurrencyToken { get; set; }

    public ICollection<AddressDto>? Addresses { get; set; }
    public ICollection<OperatorContactDataDto>? ContactData { get; set; }
    public ICollection<OperatorInterventionTypeDto>? InterventionTypes { get; set; }
    public ICollection<EntityLabelDto>? Labels { get; set; }
    public ICollection<EntityAttachmentDto>? Attachments { get; set; }
}