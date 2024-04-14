using AutoMapper;
using Regira.Fleet.Models.EntityLabels;
using Regira.Fleet.Models.InterventionOperators;
using Regira.Fleet.Models.Interventions;
using Regira.Fleet.Models.InterventionTypes;
using Regira.Fleet.Models.Vehicles;

namespace Regira.Fleet.Models;

public class FleetProfile : Profile
{
    public FleetProfile()
    {
        var _ = new Profile[] {
            new EntityLabelProfile(),
            new VehicleProfile(),
            new OperatorProfile(),
            new InterventionTypeProfile(),
            new InterventionProfile()
        };
    }
}