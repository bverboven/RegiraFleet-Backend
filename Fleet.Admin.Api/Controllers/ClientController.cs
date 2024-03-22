using Microsoft.AspNetCore.Mvc;
using Regira.Entities.Models;
using Regira.Entities.Web.Controllers.Abstractions;
using Regira.Entities.Web.Models;
using Regira.Fleet.Identity.Entities.Clients;

namespace Fleet.Admin.Api.Controllers;

[ApiController]
[Route("clients")]
public class ClientController() : EntityControllerBase<Client, string, ClientSearchObject, EntitySortBy, ClientIncludes, ClientDto, ClientInputDto>
{
    [HttpPost]
    public override async Task<ActionResult<SaveResult<ClientDto>>> Create([FromBody] ClientInputDto model)
    {
        return await this.Save(model);
    }
}
