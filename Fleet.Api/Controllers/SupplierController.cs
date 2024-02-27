using Microsoft.AspNetCore.Mvc;
using Regira.Entities.Models;
using Regira.Entities.Web.Controllers.Abstractions;
using Regira.Fleet.Api.Models.Input;
using Regira.Fleet.Suppliers;

namespace Regira.Fleet.Api.Controllers;

[ApiController]
[Route("suppliers")]
public class SupplierController : EntityControllerBase<Supplier, SupplierSearchObject, EntitySortBy, EntityIncludes, Supplier, SupplierInputDto>
{
}