using Regira.Entities.Web.Attachments.Models;
using Regira.Fleet.Entities.Suppliers.Addresses;
using Regira.Fleet.Entities.Suppliers.ContactData;
using Regira.Fleet.Entities.Suppliers.SupplierTypes;

namespace Regira.Fleet.Entities.Suppliers;

public class SupplierDto
{
    public int Id { get; set; }
    public string Guid { get; set; } = null!;
    public int ClientId { get; set; }
    public int? SupplierTypeId { get; set; }
    public string? Code { get; set; }
    public string? Title { get; set; }

    public string? IdentificationNumber { get; set; }

    public string? Description { get; set; }
    public string? Notes { get; set; }
    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
    public bool IsArchived { get; set; }

    public SupplierTypeDto? SupplierType { get; set; }
    public ICollection<AddressDto>? Addresses { get; set; }
    public ICollection<SupplierContactDataDto>? ContactData { get; set; }
    public ICollection<EntityAttachmentDto>? Attachments { get; set; }
}