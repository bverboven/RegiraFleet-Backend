using Microsoft.EntityFrameworkCore;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Data;

namespace Regira.Fleet.Entities.Clients;

public class ClientContext(FleetContext dbContext) : IClientContext
{
    public Client? Client { get; private set; }
    public int ClientId => Client?.Id ?? throw new Exception("Client not loaded");

    public async Task Load(string clientId)
        => Client = (await dbContext.Clients.SingleOrDefaultAsync(x => x.Guid == clientId)) ?? throw new Exception($"Client '{clientId}' not found");
}
