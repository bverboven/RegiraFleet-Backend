using Microsoft.AspNetCore.Mvc;
using Regira.Entities.Web.Controllers.Abstractions;
using Regira.Fleet.Entities.Vehicles.VehicleTypes;

namespace Regira.Fleet.Api.Controllers;

[ApiController]
[Route("vehicle-types")]
public class VehicleTypeController : EntityControllerBase<VehicleType, VehicleTypeSearchObject, VehicleTypeDto, VehicleTypeInputDto>
{
}