using Regira.Entities.Models;

namespace Regira.Fleet.Bookings;

public class BookingSearchObject : SearchObject
{
    public ICollection<int>? CarId { get; set; }
    public ICollection<int>? SupplierId { get; set; }
    public ICollection<int>? InterventionTypeId { get; set; }
}