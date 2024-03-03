using System.Security.Claims;

namespace Regira.Fleet.Core.Abstractions;

public interface IClientUserClaimsService
{
    Task Process(ClaimsIdentity identity);
}
