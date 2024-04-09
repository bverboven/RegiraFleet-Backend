namespace Regira.Fleet.Models.Interventions.Invoices;

public class InvoiceDto
{
    public int Id { get; set; }
    public string? InvoiceNumber { get; set; }
    public DateTime? InvoiceDate { get; set; }
    public TaxCategory? TaxCategory { get; set; }
    public decimal? TaxAmount { get; set; }
    public decimal? PriceExcl { get; set; }
    public decimal? PriceIncl { get; set; }
    public string? Description { get; set; }
}