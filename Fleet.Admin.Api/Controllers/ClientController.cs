using Microsoft.AspNetCore.Mvc;
using Regira.Entities.Models;
using Regira.Entities.Web.Controllers.Abstractions;
using Regira.Fleet.Identity.Models.Clients;

namespace Regira.Fleet.Admin.Api.Controllers;

[ApiController]
[Route("clients")]
public class ClientController : EntityControllerBase<Client, string, ClientSearchObject, EntitySortBy, ClientIncludes, ClientDto, ClientInputDto>;
