using Microsoft.AspNetCore.Mvc;
using Regira.Entities.Web.Controllers.Abstractions;
using Regira.Fleet.Api.Models.Input;
using Regira.Fleet.Entities.Suppliers.SupplierTypes;

namespace Regira.Fleet.Api.Controllers;

[ApiController]
[Route("supplier-types")]
public class SupplierTypeController : EntityControllerBase<SupplierType, SupplierTypeSearchObject, SupplierType, SupplierTypeInputDto>
{
}