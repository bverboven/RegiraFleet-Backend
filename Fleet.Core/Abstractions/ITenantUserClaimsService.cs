using System.Security.Claims;

namespace Regira.Fleet.Core.Abstractions;

public interface ITenantUserClaimsService
{
    Task Process(ClaimsIdentity identity);
}
