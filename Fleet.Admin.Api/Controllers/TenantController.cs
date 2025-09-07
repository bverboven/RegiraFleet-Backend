using Microsoft.AspNetCore.Mvc;
using Regira.Entities.Models;
using Regira.Entities.Web.Controllers.Abstractions;
using Regira.Fleet.Identity.Models.Tenants;

namespace Regira.Fleet.Admin.Api.Controllers;

[ApiController]
[Route("tenants")]
public class TenantController : EntityControllerBase<Tenant, string, TenantSearchObject, EntitySortBy, TenantIncludes, TenantDto, TenantInputDto>;
