using Microsoft.AspNetCore.Mvc;
using Regira.Entities.Web.Controllers.Abstractions;
using Regira.Fleet.Models.Vehicles.VehicleTypes;

namespace Regira.Fleet.Manager.Api.Controllers;

[ApiController]
[Route("vehicle-types")]
public class VehicleTypeController : EntityControllerBase<VehicleType, VehicleTypeSearchObject, VehicleTypeDto, VehicleTypeInputDto>
{
}