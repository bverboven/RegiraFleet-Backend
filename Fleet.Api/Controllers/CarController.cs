using Microsoft.AspNetCore.Mvc;
using Regira.Entities.Models;
using Regira.Entities.Web.Attachments.Abstractions;
using Regira.Entities.Web.Attachments.Models;
using Regira.Entities.Web.Controllers.Abstractions;
using Regira.Fleet.Entities.Cars;

namespace Regira.Fleet.Api.Controllers;

[ApiController]
[Route("cars")]
public class CarController : EntityControllerBase<Car, CarSearchObject, EntitySortBy, EntityIncludes, Car, CarInputDto>
{
}

[ApiController]
[Route("cars")]
public class CarAttachmentController : EntityAttachmentControllerBase<CarAttachment, EntityAttachmentDto, EntityAttachmentInputDto>
{
}