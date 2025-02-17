using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Core.Constants;

namespace Regira.Fleet.Clients;

public class ClientContext(IHttpContextAccessor httpContextAccessor) : IClientContext
{
    public string? ClientId => httpContextAccessor.HttpContext?.User.FindFirstValue(FleetClaimTypes.ClientId);
}

public class WritableClientContext : IClientContext
{
    public string? ClientId { get; set; }
}