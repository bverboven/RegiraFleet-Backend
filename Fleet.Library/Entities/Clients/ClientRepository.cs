using Regira.Fleet.Abstractions;
using Regira.Fleet.Core.Abstractions;
using Regira.Fleet.Data;

namespace Regira.Fleet.Entities.Clients;

public class ClientRepository(FleetContext dbContext, IFleetAppContext appContext) : FleetRepositoryBase<Client, ClientSearchObject>(dbContext, appContext)
{
}
