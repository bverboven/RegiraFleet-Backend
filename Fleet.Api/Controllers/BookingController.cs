using Microsoft.AspNetCore.Mvc;
using Regira.Entities.Models;
using Regira.Entities.Web.Controllers.Abstractions;
using Regira.Fleet.Api.Models.Input;
using Regira.Fleet.Entities.Interventions;

namespace Regira.Fleet.Api.Controllers;

[ApiController]
[Route("bookings")]
public class BookingController : EntityControllerBase<Intervention, InterventionSearchObject, EntitySortBy, InterventionIncludes, Intervention, InterventionInputDto>
{
}