using Microsoft.EntityFrameworkCore;
using Regira.Entities.Models;
using Regira.Fleet.DAL.Abstractions;
using Regira.Fleet.Data;

namespace Regira.Fleet.Bookings;

public class BookingRepository(FleetContext dbContext) : DefaultFleetRepository<Booking, BookingSearchObject, EntitySortBy, BookingIncludes>(dbContext)
{
    public override IQueryable<Booking> Filter(IQueryable<Booking> query, BookingSearchObject? so)
    {
        query = base.Filter(query, so);

        if (so?.CarId?.Any() == true)
        {
            query = query.Where(x => so.CarId.Contains(x.CarId!.Value));
        }
        if (so?.SupplierId?.Any() == true)
        {
            query = query.Where(x => so.SupplierId.Contains(x.SupplierId!.Value));
        }
        if (so?.InterventionTypeId?.Any() == true)
        {
            query = query.Where(x => so.InterventionTypeId.Contains(x.InterventionTypeId!.Value));
        }

        return query;
    }
    public override IQueryable<Booking> SortBy(IQueryable<Booking> query, EntitySortBy? sortBy = null)
    {
        return query
            .OrderByDescending(x => x.InvoiceDate)
            .ThenByDescending(x => x.Id);
    }
    public override IQueryable<Booking> AddIncludes(IQueryable<Booking> query, BookingIncludes? includes)
    {
        query = base.AddIncludes(query, includes);

        if (includes.HasValue)
        {
            if (includes.Value.HasFlag(BookingIncludes.Cars))
            {
                query = query
                    .Include(x => x.Car)
                    .ThenInclude(c => c.CarType)
                    .Include(x => x.Car)
                    .ThenInclude(c => c.Brand);
            }
            if (includes.Value.HasFlag(BookingIncludes.Suppliers))
            {
                query = query
                    .Include(x => x.Supplier)
                    .ThenInclude(s => s.SupplierType);
            }
            if (includes.Value.HasFlag(BookingIncludes.InterventionTypes))
            {
                query = query
                    .Include(x => x.InterventionType);
            }
        }

        return query;
    }
}