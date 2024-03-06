using Bogus;
using Microsoft.EntityFrameworkCore;
using Regira.Entities.Abstractions;
using Regira.Fleet.Data;
using Regira.Fleet.Entities.Clients;
using Regira.Fleet.Entities.InterventionOperators.Addresses;
using Regira.Fleet.Entities.InterventionOperators.ContactData;
using Regira.Fleet.Entities.InterventionOperators.Operators;
using Regira.Fleet.Entities.Interventions;
using Regira.Fleet.Entities.InterventionTypes;
using Regira.Fleet.Entities.Vehicles;
using Regira.Fleet.Entities.Vehicles.Brands;
using Regira.Fleet.Entities.Vehicles.VehicleTypes;
using Regira.Utilities;
using Regira.Web.Utilities;

namespace Fleet.EfCoreConsole;

public class DataSeeder(FleetContext dbContext, IEntityService<Brand> brandService, IEntityService<Intervention> interventionService,
    IEntityService<InterventionType> interventionTypeService, IEntityService<VehicleType> vehicleTypeService,
    IEntityService<Operator> operatorService, IEntityService<Vehicle> vehicleService)
{
    Dictionary<string, string> CarBrands => new()
    {
        {"ALF", "Alfa Romeo"},
        {"AUD", "Audi"},
        {"BYD", "BYD"},
        {"CIT", "Citroën"},
        {"DAC", "Dacia"},
        {"DSA", "DS Automobiles"},
        {"FIA", "Fiat"},
        {"FOR", "Ford"},
        {"HON", "Honda"},
        {"HYU", "Hyundai"},
        {"JAG", "Jaguar"},
        {"KIA", "Kia"},
        {"LRV", "Land Rover"},
        {"MAZ", "Mazda"},
        {"MBZ", "Mercedes-Benz"},
        {"MIN", "Mini"},
        {"NIS", "Nissan"},
        {"OPL", "Opel"},
        {"PGT", "Peugeot"},
        {"PLS", "Polestar"},
        {"POR", "Porsche"},
        {"RNT", "Renault"},
        {"SKO", "Skoda"},
        {"SUZ", "Suzuki"},
        {"TES", "Tesla"},
        {"TOY", "Toyota"},
        {"VWG", "Volkswagen"},
        {"VLV", "Volvo"}
    };
    private IList<Client> _clients = null!;
    private Client PublicTransport => _clients.Single(x => x.Code == "TRA");
    private Client Police => _clients.Single(x => x.Code == "POL");
    private Client FireBrigade => _clients.Single(x => x.Code == "BWR");
    private Client Ambulance => _clients.Single(x => x.Code == "AMB");


    public async Task<IList<Client>> Seed()
    {
        _clients = await SeedClients();
        await SeedBrands();
        await SeedInterventionTypes();
        await SeedVehicleTypes();
        foreach (var client in _clients)
        {
            await SeedOperators(client.Id);
            await SeedVehicles(client.Id);
            await SeedInterventions(client.Id);
        }

        return _clients;
    }

    public async Task<IList<Client>> SeedClients()
    {
        var items = await dbContext.Clients.ToListAsync();
        if (!items.Any())
        {
            items.AddRange(new Client[]
            {
                new() { Code = "TRA", Title = "Openbaar vervoer", Guid = "11fc2d46df234aed8df1de9a7b0f114f" },
                new() { Code = "POL", Title = "Politie", Guid = "232dfc2012b8491cb7d1aaee93007480" },
                new() { Code = "BWR", Title = "Brandweer", Guid = "1b615c0096c04eb2975ef84463aa8257" },
                new() { Code = "AMB", Title = "Ambulance", Guid = "f64a75e938b64dfaae5eab03fe541972" }
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
            items.AddRange(CarBrands.Select(b => new Brand { ClientId = Police.Id, Code = b.Key, Title = b.Value }));
            items.AddRange(new Brand[]
            {
                new() { ClientId = FireBrigade.Id, Code = "BMW", Title = "BMW" },
                new() { ClientId = FireBrigade.Id, Code = "FOR", Title = "Ford" },
                new() { ClientId = FireBrigade.Id, Code = "MER", Title = "Mercedes" },
                new() { ClientId = FireBrigade.Id, Code = "PEU", Title = "Peugeot" },
                new() { ClientId = FireBrigade.Id, Code = "REN", Title = "Renault" },
                new() { ClientId = FireBrigade.Id, Code = "VLV", Title = "Volvo" },
            });
            items.AddRange(new Brand[]
            {
                new() { ClientId = Ambulance.Id, Code = "BMW", Title = "BMW" },
                new() { ClientId = Ambulance.Id, Code = "FOR", Title = "Ford" },
                new() { ClientId = Ambulance.Id, Code = "FIA", Title = "Fiat" },
                new() { ClientId = Ambulance.Id, Code = "MER", Title = "Mercedes" },
                new() { ClientId = Ambulance.Id, Code = "PEU", Title = "Peugeot" },
                new() { ClientId = Ambulance.Id, Code = "REN", Title = "Renault" },
                new() { ClientId = Ambulance.Id, Code = "VLV", Title = "Volvo" },
            });

            foreach (var item in items)
            {
                await brandService.Add(item);
            }
            await brandService.SaveChanges();
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

            foreach (var item in items)
            {
                await interventionTypeService.Add(item);
            }
            await interventionTypeService.SaveChanges();
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

            foreach (var item in items)
            {
                await vehicleTypeService.Add(item);
            }
            await vehicleTypeService.SaveChanges();
        }
    }
    public async Task SeedOperators(int clientId)
    {
        var interventionTypes = await dbContext.InterventionTypes
            .Where(x => x.ClientId == clientId)
            .AsNoTracking()
            .ToArrayAsync();

        var addressRule = new Faker<OperatorAddress>("nl_BE")
            .RuleFor(x => x.CountryCode, f => f.Address.CountryCode())
            .RuleFor(x => x.PostalCode, (f, x) => f.Address.ZipCode())
            .RuleFor(x => x.City, (f, x) => f.Address.City())
            .RuleFor(x => x.Street, (f, x) => f.Address.StreetAddress())
            .RuleFor(x => x.Number, (f, x) => f.Address.BuildingNumber());

        var items = new Faker<Operator>("nl_BE")
            .RuleFor(x => x.ClientId, _ => clientId)
            .RuleFor(x => x.Title, (f, x) => f.Company.CompanyName())
            .RuleFor(x => x.IdentificationNumber, f => f.Random.Bool(.6f) ? "BE" + f.Random.Number(999, 999999999).ToString().PadLeft(10, '0') : null)
            .RuleFor(x => x.Addresses, (f, x) => addressRule.Generate(f.Random.Number(0, 2)).ToList())
            .RuleFor(x => x.ContactData, (f, x) => Enumerable.Range(0, f.Random.Number(0, 2)).Select(_ => GenerateContactData(f, x)).ToList())
            .RuleFor(x => x.InterventionTypes, (f) => Enumerable.Range(0, f.Random.Number(0, interventionTypes.Length))
                .Select(_ => new OperatorInterventionType { InterventionTypeId = f.PickRandom(interventionTypes).Id })
                .DistinctBy(x => x.InterventionTypeId).ToList()
            )
            .Generate(100)!;

        foreach (var item in items)
        {
            await operatorService.Add(item);
        }
        await operatorService.SaveChanges();
    }
    public async Task SeedVehicles(int clientId)
    {
        var brands = await dbContext.Brands
            .Where(x => x.ClientId == clientId)
            .AsNoTracking()
            .ToArrayAsync();
        var types = await dbContext.VehicleTypes
            .Where(x => x.ClientId == clientId)
            .AsNoTracking()
            .ToArrayAsync();

        var codes = new Queue<int>(Enumerable.Range(0, 1000).Select((_, i) => i + 1).Shuffle().Take(100));

        var items = new Faker<Vehicle>()
            .RuleFor(x => x.ClientId, _ => clientId)
            .RuleFor(x => x.Code, _ => codes.Dequeue().ToString().PadLeft(3, '0'))
            .RuleFor(x => x.BrandId, f => f.PickRandom(brands).Id)
            .RuleFor(x => x.Model, (f, x) => f.Vehicle.Model())
            .RuleFor(x => x.VehicleTypeId, f => f.PickRandom(types).Id)
            .Generate(100);

        foreach (var item in items)
        {
            await vehicleService.Add(item);
        }
        await vehicleService.SaveChanges();
    }
    public async Task SeedInterventions(int clientId)
    {
        var vehicles = await dbContext.Vehicles
            .Where(x => x.ClientId == clientId)
            .AsNoTracking()
            .ToArrayAsync();
        var suppliers = await dbContext.InterventionOperators
            .Where(x => x.ClientId == clientId && x.InterventionTypes!.Any())
            .AsNoTracking()
            .ToArrayAsync();
        var types = await dbContext.InterventionTypes
            .Where(x => x.ClientId == clientId)
            .AsNoTracking()
            .ToArrayAsync();

        var items = new Faker<Intervention>()
            .RuleFor(x => x.ClientId, _ => clientId)
            .RuleFor(x => x.VehicleId, (f) => f.PickRandom(vehicles).Id)
            .RuleFor(x => x.OperatorId, (f) => f.PickRandom(suppliers).Id)
            .RuleFor(x => x.InterventionTypes, (f, x) => (suppliers.FirstOrDefault(s => s.Id == x.OperatorId)
                ?.InterventionTypes
                ?.Shuffle()
                .Take(f.Random.Number(1, 3))
                .Select(t => new InterventionInterventionType { InterventionTypeId = t.InterventionTypeId })
                .ToList())
                ?? new() { new InterventionInterventionType { InterventionTypeId = f.PickRandom(types).Id } }
            )
            .RuleFor(x => x.Mileage, (f) => (int)(Math.Floor((decimal)f.Random.Number(0, 999_999) / 1000) * 1000))
            .RuleFor(x => x.InterventionDate, f => f.Date.Between(DateTime.Today.AddYears(-5), DateTime.Today))
            .Generate(vehicles.Length * 5);

        foreach (var item in items)
        {
            await interventionService.Add(item);
        }
        await interventionService.SaveChanges();
    }



    OperatorContactData GenerateContactData(Faker f, Operator supplier)
    {
        var cd = new OperatorContactData
        {
            DataType = f.PickRandom(EnumUtility.ListValidFlagValues<ContactDataTypes>().Where(x => x != ContactDataTypes.Other))
        };
        switch (cd.DataType)
        {
            case ContactDataTypes.Email:
                var provider = $"{UriUtility.Slugify(supplier.Title)}.{f.Internet.DomainSuffix()}";
                cd.Value = f.Internet.Email(provider: provider ?? f.Internet.DomainName()).ToLowerInvariant();
                break;
            case ContactDataTypes.Phone:
                cd.Value = f.Phone.PhoneNumber("0## ## ## ##");
                break;
            case ContactDataTypes.Website:
                cd.Value = $"{f.PickRandom(new[] { "www.", "services.", "business", "sales", "" })}{UriUtility.Slugify(supplier.Title!)}.{f.Internet.DomainSuffix()}";
                break;
        }

        return cd;
    }
}