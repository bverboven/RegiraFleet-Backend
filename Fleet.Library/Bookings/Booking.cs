using System.ComponentModel.DataAnnotations;
using Regira.Entities.Models.Abstractions;
using Regira.Fleet.Cars;
using Regira.Fleet.InterventionTypes;
using Regira.Fleet.Suppliers;

namespace Regira.Fleet.Bookings;

public class Booking : IEntityWithSerial
{
    public int Id { get; set; }
    public int? CarId { get; set; }
    public int? SupplierId { get; set; }
    public int? InterventionTypeId { get; set; }

    [MaxLength(32)]
    public string? InvoiceNumber { get; set; }
    public DateTime? InvoiceDate { get; set; }
    [MaxLength(1)]
    public string? TaxCategory { get; set; }
    public decimal? TaxAmount { get; set; }
    public decimal? PriceExcl { get; set; }
    public decimal? PriceIncl { get; set; }
    public int? Mileage { get; set; }
    [MaxLength(256)]
    public string? Hyperlink { get; set; }
    [MaxLength(256)]
    public string? Comments { get; set; }
    public string? Notes { get; set; }


    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime LastModified { get; set; } = DateTime.Now;


    public Car? Car { get; set; }
    public Supplier? Supplier { get; set; }
    public InterventionType? InterventionType { get; set; }
}