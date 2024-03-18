using Microsoft.AspNetCore.Mvc;
using Regira.Entities.Models;
using Regira.Entities.Web.Controllers.Abstractions;
using Regira.Fleet.Entities.Clients;

namespace Fleet.Admin.Api.Controllers;

[ApiController]
[Route("admin/clients")]
public class ClientController() : EntityControllerBase<Client, ClientSearchObject, EntitySortBy, EntityIncludes, ClientDto, ClientInputDto>
{
}
