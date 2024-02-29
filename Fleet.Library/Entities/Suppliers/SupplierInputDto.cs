using Regira.Entities.Web.Attachments.Models;
using Regira.Fleet.Entities.Interventions.InterventionTypes;
using Regira.Fleet.Entities.Suppliers.Addresses;
using Regira.Fleet.Entities.Suppliers.ContactData;
using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Entities.Suppliers;

public class SupplierInputDto
{
    [MaxLength(16)]
    public string? Code { get; set; }
    [MaxLength(128)]
    public string? Title { get; set; }

    [MaxLength(32)]
    public string? IdentificationNumber { get; set; }
    [MaxLength(512)]
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public bool IsArchived { get; set; }
    public AddressInputDto? Address { get; set; }
    public ICollection<SupplierContactDataInputDto>? ContactData { get; set; }
    public ICollection<InterventionTypeInputDto>? InterventionTypes { get; set; }
    public ICollection<EntityAttachmentInputDto>? Attachments { get; set; }
}