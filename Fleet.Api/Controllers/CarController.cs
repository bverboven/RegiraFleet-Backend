using Microsoft.AspNetCore.Mvc;
using Regira.Entities.Models;
using Regira.Entities.Web.Controllers.Abstractions;
using Regira.Fleet.Api.Models.Input;
using Regira.Fleet.Cars;

namespace Regira.Fleet.Api.Controllers;

[ApiController]
[Route("cars")]
public class CarController : EntityControllerBase<Car, CarSearchObject, EntitySortBy, EntityIncludes, Car, CarInputDto>
{
}