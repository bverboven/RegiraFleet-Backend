using System.ComponentModel.DataAnnotations;

namespace Regira.Fleet.Api.Models.Input;

public class BookingInputDto
{
    [Required]
    public int CarId { get; set; }
    [Required]
    public int SupplierId { get; set; }
    [Required]
    public int InterventionTypeId { get; set; }

    [MaxLength(32)]
    public string? InvoiceNumber { get; set; }
    public DateTime? InvoiceDate { get; set; }
    public decimal? PriceExcl { get; set; }
    public decimal? PriceIncl { get; set; }
    [MaxLength(1)]
    public string? TaxCategory { get; set; }
    public int? Mileage { get; set; }
    [MaxLength(256)]
    public string? Hyperlink { get; set; }
    [MaxLength(256)]
    public string? Comments { get; set; }
    public string? Notes { get; set; }
}