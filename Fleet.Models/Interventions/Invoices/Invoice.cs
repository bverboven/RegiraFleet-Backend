using Regira.Entities.Models.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Models.Interventions.Invoices;

public class Invoice : IEntityWithSerial
{
    public int Id { get; set; }
    public int InterventionId { get; set; }
    [MaxLength(32)]
    public string? InvoiceNumber { get; set; }
    public DateTime? InvoiceDate { get; set; }
    public TaxCategory? TaxCategory { get; set; }
    public decimal? TaxAmount { get; set; }
    public decimal? PriceExcl { get; set; }
    public decimal? PriceIncl { get; set; }
    public string? Description { get; set; }
}