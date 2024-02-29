using Microsoft.AspNetCore.Mvc;
using Regira.Entities.Models;
using Regira.Entities.Web.Attachments.Abstractions;
using Regira.Entities.Web.Attachments.Models;
using Regira.Entities.Web.Controllers.Abstractions;
using Regira.Fleet.Entities.Vehicles;

namespace Regira.Fleet.Api.Controllers;

[ApiController]
[Route("vehicles")]
public class VehicleController : EntityControllerBase<Vehicle, VehicleSearchObject, EntitySortBy, EntityIncludes, VehicleDto, VehicleInputDto>
{
}

[ApiController]
[Route("vehicles")]
public class VehicleAttachmentController : EntityAttachmentControllerBase<VehicleAttachment, EntityAttachmentDto, EntityAttachmentInputDto>
{
}