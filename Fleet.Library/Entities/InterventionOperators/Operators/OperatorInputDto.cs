using Regira.Entities.Web.Attachments.Models;
using Regira.Fleet.Entities.Addresses;
using Regira.Fleet.Entities.InterventionOperators.ContactData;
using Regira.Fleet.Entities.InterventionTypes;
using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Entities.InterventionOperators.Operators;

public class OperatorInputDto
{
    public int Id { get; set; }
    [MaxLength(8)]
    public string? Code { get; set; }
    [MaxLength(128)]
    public string Title { get; set; } = null!;

    [MaxLength(32)]
    public string? IdentificationNumber { get; set; }
    public string? Description { get; set; }
    public bool IsArchived { get; set; }
    public ICollection<AddressInputDto>? Addresses { get; set; }
    public ICollection<OperatorContactDataInputDto>? ContactData { get; set; }
    public ICollection<InterventionTypeInputDto>? InterventionTypes { get; set; }
    public ICollection<EntityAttachmentInputDto>? Attachments { get; set; }
}