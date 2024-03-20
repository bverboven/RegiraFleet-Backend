using AutoMapper;
using Regira.Fleet.Core.Constants;

namespace Regira.Fleet.Identity.Models;

public class IdentityProfile : Profile
{
    public IdentityProfile()
    {
        CreateMap<FleetUser, FleetUserDto>()
            .ForMember(e => e.Claims, e => e.Ignore())
            .AfterMap((model, dto) =>
            {
                dto.Claims = model.UserClaims?.Select(x => new FleetClaimDto { Type = x.ClaimType!, Value = x.ClaimValue }).ToList();
                dto.GivenName = dto.Claims?.FirstOrDefault(c => c.Type == FleetClaimTypes.GivenName)?.Value;
                dto.LastName = dto.Claims?.FirstOrDefault(c => c.Type == FleetClaimTypes.LastName)?.Value;
            });
    }
}
