using Microsoft.AspNetCore.Mvc;
using Regira.Entities.Models;
using Regira.Entities.Web.Controllers.Abstractions;
using Regira.Fleet.Api.Models.Input;
using Regira.Fleet.Brands;

namespace Regira.Fleet.Api.Controllers;

[ApiController]
[Route("brands")]
public class BrandController : EntityControllerBase<Brand, BrandSearchObject, EntitySortBy, EntityIncludes, Brand, BrandInputDto>
{
}