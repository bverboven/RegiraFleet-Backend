using Microsoft.AspNetCore.Mvc;
using Regira.Entities.Models;
using Regira.Entities.Web.Attachments.Abstractions;
using Regira.Entities.Web.Attachments.Models;
using Regira.Entities.Web.Controllers.Abstractions;
using Regira.Fleet.Models.Vehicles;

namespace Regira.Fleet.Manager.Api.Controllers;

[ApiController]
[Route("vehicles")]
public class VehicleController : EntityControllerBase<Vehicle, VehicleSearchObject, EntitySortBy, VehicleIncludes, VehicleDto, VehicleInputDto>
{
}

[ApiController]
[Route("vehicles")]
public class VehicleAttachmentController : EntityAttachmentControllerBase<VehicleAttachment, EntityAttachmentDto, EntityAttachmentInputDto>
{
}