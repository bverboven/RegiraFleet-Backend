using Regira.Entities.Web.Attachments.Models;
using Regira.Fleet.Models.EntityLabels;
using Regira.Fleet.Models.InterventionOperators.Operators;
using Regira.Fleet.Models.Interventions.Invoices;
using Regira.Fleet.Models.InterventionTypes;
using Regira.Fleet.Models.Vehicles;

namespace Regira.Fleet.Models.Interventions;

public class InterventionDto
{
    public int Id { get; set; }
    public string Guid { get; set; } = null!;
    public string ClientId { get; set; } = null!;
    public int VehicleId { get; set; }
    public int OperatorId { get; set; }
    public int? InvoiceId { get; set; }
    public int? InterventionTypeId { get; set; }
    public DateTime? InterventionDate { get; set; }
    public int? Mileage { get; set; }
    public string? Description { get; set; }

    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }


    public VehicleDto? Vehicle { get; set; }
    public OperatorDto? Operator { get; set; }
    public InvoiceDto? Invoice { get; set; }
    public InterventionTypeDto? InterventionType { get; set; }
    public ICollection<EntityLabelDto>? Labels { get; set; }
    public ICollection<EntityAttachmentDto>? Attachments { get; set; }
}