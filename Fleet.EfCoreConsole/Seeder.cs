using Microsoft.EntityFrameworkCore;
using Regira.Fleet.Data;
using Regira.Fleet.Entities.Clients;
using Regira.Fleet.Entities.Interventions.InterventionTypes;
using Regira.Fleet.Entities.Vehicles.Brands;
using Regira.Fleet.Entities.Vehicles.VehicleTypes;

namespace Fleet.EfCoreConsole;

public class Seeder(FleetContext dbContext)
{
    private IList<Client> _clients = null!;
    private Client PublicTransport => _clients.Single(x => x.Code == "TRA");
    private Client Police => _clients.Single(x => x.Code == "POL");
    private Client FireBrigade => _clients.Single(x => x.Code == "BDW");
    private Client Ambulance => _clients.Single(x => x.Code == "AMB");


    public async Task Seed()
    {
        _clients = await SeedClients();
        await SeedBrands();
        await SeedInterventionTypes();
        await SeedVehicleTypes();
    }

    public async Task<IList<Client>> SeedClients()
    {
        var items = await dbContext.Clients.ToListAsync();
        if (!items.Any())
        {
            items.AddRange(new Client[]
            {
                new() { Code = "TRA", Title = "Openbaar vervoer" },
                new() { Code = "POL", Title = "Politie" },
                new() { Code = "BDW", Title = "Brandweer" },
                new() { Code = "AMB", Title = "Ambulance" }
            });
            dbContext.Clients.AddRange(items);
            await dbContext.SaveChangesAsync();
        }

        return items;
    }
    public async Task SeedBrands()
    {
        var items = await dbContext.Brands.ToListAsync();
        if (!items.Any())
        {
            items.AddRange(new Brand[]
            {
                new() { ClientId = PublicTransport.Id, Code = "BYD", Title = "BYD" },
                new() { ClientId = PublicTransport.Id, Code = "CIT", Title = "Citroën" },
                new() { ClientId = PublicTransport.Id, Code = "HOO", Title = "Van Hool" },
                new() { ClientId = PublicTransport.Id, Code = "REN", Title = "Renault" },
            });
            items.AddRange(new Brand[]
            {
                new() { ClientId = Police.Id, Code = "BMW", Title = "BMW" },
                new() { ClientId = Police.Id, Code = "FOR", Title = "Ford" },
                new() { ClientId = Police.Id, Code = "MER", Title = "Mercedes" },
                new() { ClientId = Police.Id, Code = "PEU", Title = "Peugeot" },
                new() { ClientId = Police.Id, Code = "REN", Title = "Renault" },
            });
            items.AddRange(new Brand[]
            {
                new() { ClientId = FireBrigade.Id, Code = "BMW", Title = "BMW" },
                new() { ClientId = FireBrigade.Id, Code = "FOR", Title = "Ford" },
                new() { ClientId = FireBrigade.Id, Code = "MER", Title = "Mercedes" },
                new() { ClientId = FireBrigade.Id, Code = "PEU", Title = "Peugeot" },
                new() { ClientId = FireBrigade.Id, Code = "REN", Title = "Renault" },
            });
            items.AddRange(new Brand[]
            {
                new() { ClientId = Ambulance.Id, Code = "BMW", Title = "BMW" },
                new() { ClientId = Ambulance.Id, Code = "FOR", Title = "Ford" },
                new() { ClientId = Ambulance.Id, Code = "FIA", Title = "Fiat" },
                new() { ClientId = Ambulance.Id, Code = "MER", Title = "Mercedes" },
                new() { ClientId = Ambulance.Id, Code = "PEU", Title = "Peugeot" },
                new() { ClientId = Ambulance.Id, Code = "REN", Title = "Renault" },
            });

            dbContext.Brands.AddRange(items);
            await dbContext.SaveChangesAsync();
        }
    }
    public async Task SeedInterventionTypes()
    {
        var items = await dbContext.InterventionTypes.ToListAsync();
        if (!items.Any())
        {
            items.AddRange(new InterventionType[]
            {
                new() { ClientId = PublicTransport.Id, Code = "MAIN", Title = "Onderhoud" },
                new() { ClientId = PublicTransport.Id, Code = "TIRE", Title = "Banden" },
                new() { ClientId = PublicTransport.Id, Code = "BRAKE", Title = "Remmen" },
                new() { ClientId = PublicTransport.Id, Code = "BODY", Title = "Carrosserie" }
            });
            items.AddRange(new InterventionType[]
            {
                new() { ClientId = Police.Id, Code = "MAIN", Title = "Onderhoud" },
                new() { ClientId = Police.Id, Code = "TIRE", Title = "Banden" },
                new() { ClientId = Police.Id, Code = "BRAKE", Title = "Remmen" },
                new() { ClientId = Police.Id, Code = "BODY", Title = "Carrosserie" }
            });
            items.AddRange(new InterventionType[]
            {
                new() { ClientId = FireBrigade.Id, Code = "MAIN", Title = "Onderhoud" },
                new() { ClientId = FireBrigade.Id, Code = "TIRE", Title = "Banden" },
                new() { ClientId = FireBrigade.Id, Code = "BRAKE", Title = "Remmen" },
                new() { ClientId = FireBrigade.Id, Code = "BODY", Title = "Carrosserie" }
            });
            items.AddRange(new InterventionType[]
            {
                new() { ClientId = Ambulance.Id, Code = "MAIN", Title = "Onderhoud" },
                new() { ClientId = Ambulance.Id, Code = "TIRE", Title = "Banden" },
                new() { ClientId = Ambulance.Id, Code = "BRAKE", Title = "Remmen" },
                new() { ClientId = Ambulance.Id, Code = "BODY", Title = "Carrosserie" }
            });
            
            dbContext.InterventionTypes.AddRange(items);
            await dbContext.SaveChangesAsync();
        }
    }
    public async Task SeedVehicleTypes()
    {
        var items = await dbContext.VehicleTypes.ToListAsync();
        if (!items.Any())
        {

            items.AddRange(new VehicleType[]
            {
                new() { ClientId = PublicTransport.Id, Code = "BUS", Title = "Bus" },
                new() { ClientId = PublicTransport.Id, Code = "TRA", Title = "Tram" },
                new() { ClientId = PublicTransport.Id, Code = "HTR", Title = "Paardentram", IsArchived = true },
                new() { ClientId = PublicTransport.Id, Code = "EXEC", Title = "Directiewagen" },
            });
            items.AddRange(new VehicleType[]
            {
                new() { ClientId = Police.Id, Code = "COMBI", Title = "Combi" },
                new() { ClientId = Police.Id, Code = "MOTOR", Title = "Motorbike" },
                new() { ClientId = Police.Id, Code = "UAV", Title = "Drone" },
                new() { ClientId = Police.Id, Code = "EXEC", Title = "Directiewagen" },
            });
            items.AddRange(new VehicleType[]
            {
                new() { ClientId = FireBrigade.Id, Code = "TRU", Title = "Brandweerwagen" },
                new() { ClientId = FireBrigade.Id, Code = "UGV", Title = "Drone (grond)" },
                new() { ClientId = FireBrigade.Id, Code = "UAV", Title = "Drone (lucht)" },
            });
            items.AddRange(new VehicleType[]
            {
                new() { ClientId = Ambulance.Id, Code = "AMB", Title = "Ziekenwagen" },
                new() { ClientId = Ambulance.Id, Code = "MOT", Title = "Motor" },
                new() { ClientId = Ambulance.Id, Code = "EXEC", Title = "Directiewagen" },
            });
            
            dbContext.VehicleTypes.AddRange(items);
            await dbContext.SaveChangesAsync();
        }
    }
}