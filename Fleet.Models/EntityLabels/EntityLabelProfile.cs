using AutoMapper;

namespace Regira.Fleet.Models.EntityLabels;

public class EntityLabelProfile : Profile
{
    public EntityLabelProfile()
    {
        CreateMap<EntityLabel, EntityLabelDto>();
        CreateMap<EntityLabelInputDto, EntityLabel>();
    }
}
