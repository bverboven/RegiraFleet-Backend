using Bogus;
using Microsoft.EntityFrameworkCore;
using Regira.Entities.Abstractions;
using Regira.Fleet.Data;
using Regira.Fleet.Identity.Models.Clients;
using Regira.Fleet.Models.InterventionOperators.Addresses;
using Regira.Fleet.Models.InterventionOperators.ContactData;
using Regira.Fleet.Models.InterventionOperators.Operators;
using Regira.Fleet.Models.Interventions;
using Regira.Fleet.Models.Interventions.Invoices;
using Regira.Fleet.Models.InterventionTypes;
using Regira.Fleet.Models.Vehicles;
using Regira.Fleet.Models.Vehicles.Brands;
using Regira.Fleet.Models.Vehicles.VehicleTypes;
using Regira.Utilities;
using Regira.Web.Utilities;

namespace DemoData.Console;

public class DataSeeder(FleetContextBase dbContext, IEntityService<Brand> brandService, IEntityService<Intervention> interventionService,
    IEntityService<InterventionType> interventionTypeService, IEntityService<VehicleType> vehicleTypeService,
    IEntityService<Operator> operatorService, IEntityService<Vehicle> vehicleService)
{
    const int FACTOR = 100;
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


    public async Task Seed(IList<Client> clients)
    {
        _clients = clients;
        await SeedBrands();
        await SeedInterventionTypes();
        await SeedVehicleTypes();
        foreach (var client in _clients)
        {
            await SeedOperators(client.Id);
            await SeedVehicles(client.Id);
            await SeedInterventions(client.Id);
        }
    }

    public async Task SeedBrands()
    {
        var items = await dbContext.VehicleBrands.ToListAsync();
        if (!items.Any())
        {
            items.AddRange([
                new() { ClientId = PublicTransport.Id, Code = "BYD", Title = "BYD" },
                new() { ClientId = PublicTransport.Id, Code = "CIT", Title = "Citroën" },
                new() { ClientId = PublicTransport.Id, Code = "HOO", Title = "Van Hool" },
                new() { ClientId = PublicTransport.Id, Code = "REN", Title = "Renault" }
            ]);
            items.AddRange(CarBrands.Select(b => new Brand { ClientId = Police.Id, Code = b.Key, Title = b.Value }));
            items.AddRange([
                new() { ClientId = FireBrigade.Id, Code = "BMW", Title = "BMW" },
                new() { ClientId = FireBrigade.Id, Code = "FOR", Title = "Ford" },
                new() { ClientId = FireBrigade.Id, Code = "MER", Title = "Mercedes" },
                new() { ClientId = FireBrigade.Id, Code = "PEU", Title = "Peugeot" },
                new() { ClientId = FireBrigade.Id, Code = "REN", Title = "Renault" },
                new() { ClientId = FireBrigade.Id, Code = "VLV", Title = "Volvo" }
            ]);
            items.AddRange([
                new() { ClientId = Ambulance.Id, Code = "BMW", Title = "BMW" },
                new() { ClientId = Ambulance.Id, Code = "FOR", Title = "Ford" },
                new() { ClientId = Ambulance.Id, Code = "FIA", Title = "Fiat" },
                new() { ClientId = Ambulance.Id, Code = "MER", Title = "Mercedes" },
                new() { ClientId = Ambulance.Id, Code = "PEU", Title = "Peugeot" },
                new() { ClientId = Ambulance.Id, Code = "REN", Title = "Renault" },
                new() { ClientId = Ambulance.Id, Code = "VLV", Title = "Volvo" }
            ]);

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
            items.AddRange([
                new() { ClientId = PublicTransport.Id, Code = "MAIN", Title = "Onderhoud" },
                new() { ClientId = PublicTransport.Id, Code = "TIRE", Title = "Banden" },
                new() { ClientId = PublicTransport.Id, Code = "BRAKE", Title = "Remmen" },
                new() { ClientId = PublicTransport.Id, Code = "BODY", Title = "Carrosserie" }
            ]);
            items.AddRange([
                new() { ClientId = Police.Id, Code = "MAIN", Title = "Onderhoud" },
                new() { ClientId = Police.Id, Code = "TIRE", Title = "Banden" },
                new() { ClientId = Police.Id, Code = "BRAKE", Title = "Remmen" },
                new() { ClientId = Police.Id, Code = "BODY", Title = "Carrosserie" },
                new() { ClientId = Police.Id, Code = "ROT", Title = "Rotor" }
            ]);
            items.AddRange([
                new() { ClientId = FireBrigade.Id, Code = "MAIN", Title = "Onderhoud" },
                new() { ClientId = FireBrigade.Id, Code = "TIRE", Title = "Banden" },
                new() { ClientId = FireBrigade.Id, Code = "BRAKE", Title = "Remmen" },
                new() { ClientId = FireBrigade.Id, Code = "BODY", Title = "Carrosserie" },
                new() { ClientId = FireBrigade.Id, Code = "ROT", Title = "Rotor" }
            ]);
            items.AddRange([
                new() { ClientId = Ambulance.Id, Code = "MAIN", Title = "Onderhoud" },
                new() { ClientId = Ambulance.Id, Code = "TIRE", Title = "Banden" },
                new() { ClientId = Ambulance.Id, Code = "BRAKE", Title = "Remmen" },
                new() { ClientId = Ambulance.Id, Code = "BODY", Title = "Carrosserie" }
            ]);

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

            items.AddRange([
                new() { ClientId = PublicTransport.Id, Code = "BUS", Title = "Bus" },
                new() { ClientId = PublicTransport.Id, Code = "TRA", Title = "Tram" },
                new() { ClientId = PublicTransport.Id, Code = "HTR", Title = "Paardentram", IsArchived = true },
                new() { ClientId = PublicTransport.Id, Code = "EXEC", Title = "Directiewagen" }
            ]);
            items.AddRange([
                new() { ClientId = Police.Id, Code = "COMBI", Title = "Combi" },
                new() { ClientId = Police.Id, Code = "MOTOR", Title = "Motorbike" },
                new() { ClientId = Police.Id, Code = "UAV", Title = "Drone" },
                new() { ClientId = Police.Id, Code = "EXEC", Title = "Directiewagen" }
            ]);
            items.AddRange([
                new() { ClientId = FireBrigade.Id, Code = "TRU", Title = "Brandweerwagen" },
                new() { ClientId = FireBrigade.Id, Code = "UGV", Title = "Drone (grond)" },
                new() { ClientId = FireBrigade.Id, Code = "UAV", Title = "Drone (lucht)" }
            ]);
            items.AddRange([
                new() { ClientId = Ambulance.Id, Code = "AMB", Title = "Ziekenwagen" },
                new() { ClientId = Ambulance.Id, Code = "MOT", Title = "Motor" },
                new() { ClientId = Ambulance.Id, Code = "EXEC", Title = "Directiewagen" }
            ]);

            foreach (var item in items)
            {
                await vehicleTypeService.Add(item);
            }
            await vehicleTypeService.SaveChanges();
        }
    }
    public async Task SeedOperators(string clientId)
    {
        var interventionTypes = await dbContext.InterventionTypes
            .Where(x => x.ClientId == clientId)
            .AsNoTracking()
            .ToArrayAsync();

        var addressRule = new Faker<OperatorAddress>("nl_BE")
            .RuleFor(x => x.CountryCode, _ => "BE")
            .RuleFor(x => x.PostalCode, (f, _) => f.Address.ZipCode())
            .RuleFor(x => x.City, (f, _) => f.Address.City())
            .RuleFor(x => x.Street, (f, _) => f.Address.StreetAddress())
            .RuleFor(x => x.Number, (f, _) => f.Address.BuildingNumber());

        var items = new Faker<Operator>("nl_BE")
            .RuleFor(x => x.ClientId, _ => clientId)
            .RuleFor(x => x.Title, (f, _) => f.Company.CompanyName())
            .RuleFor(x => x.IdentificationNumber, f => f.Random.Bool(.6f) ? "BE" + f.Random.Number(999, 999999999).ToString().PadLeft(10, '0') : null)
            .RuleFor(x => x.Addresses, (f, _) => addressRule.Generate(f.Random.Number(0, 2)).ToList())
            .RuleFor(x => x.ContactData, (f, x) => Enumerable.Range(0, f.Random.Number(0, 2)).Select(_ => GenerateContactData(f, x)).ToList())
            .RuleFor(x => x.InterventionTypes, (f) => Enumerable.Range(0, f.Random.Number(0, interventionTypes.Length))
                .Select(_ => new OperatorInterventionType { InterventionTypeId = f.PickRandom(interventionTypes).Id })
                .DistinctBy(x => x.InterventionTypeId).ToList()
            )
            .Generate(100 * FACTOR)!;

        foreach (var item in items)
        {
            await operatorService.Add(item);
        }
        await operatorService.SaveChanges();
    }
    public async Task SeedVehicles(string clientId)
    {
        var brands = await dbContext.VehicleBrands
            .Where(x => x.ClientId == clientId)
            .AsNoTracking()
            .ToArrayAsync();
        var types = await dbContext.VehicleTypes
            .Where(x => x.ClientId == clientId)
            .AsNoTracking()
            .ToArrayAsync();

        var codes = new Queue<int>(Enumerable.Range(0, 1000 * FACTOR).Select((_, i) => i + 1).Shuffle().Take(100 * FACTOR));

        var items = new Faker<Vehicle>()
            .RuleFor(x => x.ClientId, _ => clientId)
            .RuleFor(x => x.Code, _ => codes.Dequeue().ToString().PadLeft(3, '0'))
            .RuleFor(x => x.BrandId, f => f.PickRandom(brands).Id)
            .RuleFor(x => x.Model, (f, _) => f.Vehicle.Model())
            .RuleFor(x => x.VehicleTypeId, f => f.PickRandom(types).Id)
            .Generate(100 * FACTOR);

        foreach (var item in items)
        {
            await vehicleService.Add(item);
        }
        await vehicleService.SaveChanges();
    }
    public async Task SeedInterventions(string clientId)
    {
        var vehicleIds = await dbContext.Vehicles
            .Where(x => x.ClientId == clientId)
            .Select(x => x.Id)
            .ToArrayAsync();
        var suppliers = await dbContext.InterventionOperators
            .Include(x => x.InterventionTypes)
            .Where(x => x.ClientId == clientId && x.InterventionTypes!.Any())
            .Select(x => new { x.Id, InterventionTypeIds = x.InterventionTypes!.Select(y => y.InterventionTypeId) })
            .ToArrayAsync();
        var typeIds = await dbContext.InterventionTypes
            .Where(x => x.ClientId == clientId)
            .Select(x => x.Id)
            .ToArrayAsync();

        var items = new Faker<Intervention>()
            .RuleFor(x => x.ClientId, _ => clientId)
            .RuleFor(x => x.VehicleId, (f) => f.PickRandom(vehicleIds))
            .RuleFor(x => x.OperatorId, (f) => f.PickRandom(suppliers).Id)
            .RuleFor(x => x.InterventionTypeId, (f, x) => (suppliers.FirstOrDefault(s => s.Id == x.OperatorId)
                ?.InterventionTypeIds
                .Shuffle()
                .FirstOrDefault())
                ?? f.PickRandom(typeIds)
            )
            .RuleFor(x => x.Mileage, (f) => (int)(Math.Floor((decimal)f.Random.Number(0, 999_999) / 1000) * 1000))
            .RuleFor(x => x.InterventionDate, f => f.Date.Between(DateTime.Today.AddYears(-5), DateTime.Today))
            .RuleFor(x => x.Invoice, (_, x) => GenerateInvoice(x))
            .Generate(vehicleIds.Length * 5);

        foreach (var item in items)
        {
            await interventionService.Add(item);
        }
        await interventionService.SaveChanges();
    }

    Invoice GenerateInvoice(Intervention intervention)
    {
        return new Faker<Invoice>()
            .RuleFor(x => x.InvoiceNumber, f => $"INV{f.Commerce.Random.Number(1, 999999).ToString().PadLeft(8, '0')}")
            .RuleFor(x => x.InvoiceDate, f => f.Date.Between(intervention.InterventionDate!.Value, intervention.InterventionDate!.Value.AddDays(15)))
            .RuleFor(x => x.PriceExcl, f => f.Random.Decimal(10, 99_999))
            .RuleFor(x => x.PriceIncl, (_, x) => x.PriceExcl * 1.21m)
            .RuleFor(x => x.TaxAmount, (_, x) => x.PriceExcl * .21m)
            .RuleFor(x => x.TaxCategory, f => f.PickRandom(Enum.GetValues<TaxCategory>()))
            .Generate();
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
                cd.Value = f.Internet.Email(provider: provider).ToLowerInvariant().ToLower();
                break;
            case ContactDataTypes.Phone:
                cd.Value = f.Phone.PhoneNumber("0## ## ## ##");
                break;
            case ContactDataTypes.Website:
                cd.Value = $"{f.PickRandom(new[] { "www.", "services.", "business", "sales", "" })}{UriUtility.Slugify(supplier.Title)}.{f.Internet.DomainSuffix()}".ToLower();
                break;
        }

        return cd;
    }
}