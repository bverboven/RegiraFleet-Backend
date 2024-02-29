using Regira.Entities.Web.Attachments.Models;
using Regira.Fleet.Entities.Cars;
using Regira.Fleet.Entities.Interventions.InterventionTypes;
using Regira.Fleet.Entities.Interventions.Invoices;
using Regira.Fleet.Entities.Suppliers;

namespace Regira.Fleet.Entities.Interventions;

public class InterventionDto
{
    public int Id { get; set; }
    public string Guid { get; set; } = null!;
    public int ClientId { get; set; }
    public int? CarId { get; set; }
    public int? SupplierId { get; set; }
    public int? InterventionTypeId { get; set; }
    public int? InvoiceId { get; set; }
    public int? Mileage { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }


    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }


    public CarDto? Car { get; set; }
    public SupplierDto? Supplier { get; set; }
    public InterventionTypeDto? InterventionType { get; set; }
    public InvoiceDto? Invoice { get; set; }
    public ICollection<EntityAttachmentDto>? Attachments { get; set; }
}