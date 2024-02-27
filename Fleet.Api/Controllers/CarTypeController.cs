using Microsoft.AspNetCore.Mvc;
using Regira.Entities.Web.Controllers.Abstractions;
using Regira.Fleet.Api.Models.Input;
using Regira.Fleet.CarTypes;

namespace Regira.Fleet.Api.Controllers;

[ApiController]
[Route("car-types")]
public class CarTypeController : EntityControllerBase<CarType, CarTypeSearchObject, CarType, CarTypeInputDto>
{
}