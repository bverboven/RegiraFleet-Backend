using Microsoft.AspNetCore.Mvc;
using Regira.Entities.Models;
using Regira.Entities.Web.Controllers.Abstractions;
using Regira.Fleet.Entities.Countries;

namespace Regira.Fleet.Manager.Api.Controllers;

[ApiController]
[Route("countries")]
public class CountryController : EntityControllerBase<Country, string, SearchObject<string>, CountryDto, CountryDto>;