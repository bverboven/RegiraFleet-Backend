using AutoMapper;

namespace Regira.Fleet.DependencyInjection.Mapping;

public class FleetProfile : Profile
{
    public FleetProfile()
    {
        _ = new Profile[] {
            new VehicleProfile(),
            new OperatorProfile(),
            new InterventionTypeProfile(),
            new InterventionProfile()
        };
    }
}