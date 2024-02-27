using Microsoft.AspNetCore.Mvc;
using Regira.Entities.Models;
using Regira.Entities.Web.Controllers.Abstractions;
using Regira.Fleet.Api.Models.Input;
using Regira.Fleet.Bookings;

namespace Regira.Fleet.Api.Controllers;

[ApiController]
[Route("bookings")]
public class BookingController : EntityControllerBase<Booking, BookingSearchObject, EntitySortBy, BookingIncludes, Booking, BookingInputDto>
{
}