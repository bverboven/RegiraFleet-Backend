using Microsoft.AspNetCore.Http;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Core.Constants;

namespace Regira.Fleet.Clients;

public class ClientContext(IHttpContextAccessor httpContextAccessor) : IClientContext
{
    public string? ClientId => httpContextAccessor.HttpContext?.User.Claims.SingleOrDefault(c => c.Type == FleetClaimTypes.ClientId)?.Value;
}
