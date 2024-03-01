using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Entities.Interventions.Invoices;

public class InvoiceInputDto
{
    public int Id { get; set; }
    [MaxLength(32)]
    public string? InvoiceNumber { get; set; }
    public DateTime? InvoiceDate { get; set; }
    public TaxCategory? TaxCategory { get; set; }
    public decimal? TaxAmount { get; set; }
    public decimal? PriceExcl { get; set; }
    public decimal? PriceIncl { get; set; }
    public string? Notes { get; set; }
}