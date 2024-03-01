using Regira.Entities.Web.Attachments.Models;
using Regira.Fleet.Entities.InterventionTypes;
using Regira.Fleet.Entities.InterventionOperators.Addresses;
using Regira.Fleet.Entities.InterventionOperators.ContactData;
using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Entities.InterventionOperators.Operators;

public class InterventionOperatorInputDto
{
    [MaxLength(8)]
    public string? Code { get; set; }
    [MaxLength(128)]
    public string Title { get; set; } = null!;

    [MaxLength(32)]
    public string? IdentificationNumber { get; set; }
    [MaxLength(512)]
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public bool IsArchived { get; set; }
    public AddressInputDto? Address { get; set; }
    public ICollection<InterventionOperatorContactDataInputDto>? ContactData { get; set; }
    public ICollection<InterventionTypeInputDto>? InterventionTypes { get; set; }
    public ICollection<EntityAttachmentInputDto>? Attachments { get; set; }
}