using Microsoft.AspNetCore.Mvc;
using Regira.Entities.Models;
using Regira.Entities.Web.Controllers.Abstractions;
using Regira.Fleet.Models.Vehicles.Brands;

namespace Regira.Fleet.Manager.Api.Controllers;

[ApiController]
[Route("brands")]
public class BrandController : EntityControllerBase<Brand, BrandSearchObject, EntitySortBy, EntityIncludes, BrandDto, BrandInputDto>;