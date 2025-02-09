using Regira.Entities.Web.Attachments.Models;
using Regira.Fleet.Models.Addresses;
using Regira.Fleet.Models.EntityLabels;
using Regira.Fleet.Models.InterventionOperators.ContactData;
using Regira.Fleet.Models.InterventionTypes;
using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Models.InterventionOperators.Operators;

public class OperatorInputDto
{
    public int Id { get; set; }
    [MaxLength(8)]
    public string? Code { get; set; }
    [Required]
    [MaxLength(128)]
    public string Title { get; set; } = null!;

    [MaxLength(32)]
    public string? IdentificationNumber { get; set; }
    public string? Description { get; set; }
    public bool IsArchived { get; set; }
    public ICollection<AddressInputDto>? Addresses { get; set; }
    public ICollection<OperatorContactDataInputDto>? ContactData { get; set; }
    public ICollection<InterventionTypeInputDto>? InterventionTypes { get; set; }
    public ICollection<EntityLabelInputDto>? Labels { get; set; }
    public ICollection<EntityAttachmentInputDto>? Attachments { get; set; }
}