using Microsoft.AspNetCore.Mvc;
using Regira.Entities.Models;
using Regira.Entities.Web.Attachments.Abstractions;
using Regira.Entities.Web.Attachments.Models;
using Regira.Entities.Web.Controllers.Abstractions;
using Regira.Fleet.Entities.Suppliers;

namespace Regira.Fleet.Api.Controllers;

[ApiController]
[Route("suppliers")]
public class SupplierController : EntityControllerBase<Supplier, SupplierSearchObject, EntitySortBy, EntityIncludes, Supplier, SupplierInputDto>
{
}

[ApiController]
[Route("suppliers")]
public class SuppliertionAttachmentController : EntityAttachmentControllerBase<SupplierAttachment, EntityAttachmentDto, EntityAttachmentInputDto>
{
}