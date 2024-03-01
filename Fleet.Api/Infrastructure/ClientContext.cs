//using Regira.Fleet.Core.Abstractions;
//using Regira.Fleet.Data;
//using Regira.Fleet.Entities.Clients;
//using Regira.Fleet.Identity.Constants;

//namespace Regira.Fleet.Api.Infrastructure;

//public class ClientContext : IClientContext
//{
//    public Client? Client { get; private set; }
//    public int ClientId => Client?.Id ?? throw new Exception("Client not loaded");

//    public ClientContext(FleetContext dbContext, IHttpContextAccessor httpContextAccessor)
//    {
//        var clientGuid = httpContextAccessor.HttpContext?.User.Claims.Single(c => c.Type == FleetClaimTypes.ClientId).Value;
//        Client = dbContext.Clients.SingleOrDefault(x => x.Guid == clientGuid) ?? throw new Exception($"Client '{clientGuid}' not found");
//    }
//}
