using AutoMapper;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Identity.Entities.Clients;
using Regira.Fleet.Identity.Entities.Clients.Subscriptions;
using Regira.Fleet.Identity.Entities.Users;

namespace Regira.Fleet.Identity.Entities;

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
        
        CreateMap<Client, ClientDto>()
            .ForMember(e => e.Languages, e => e.Ignore())
            .AfterMap((model, dto) =>
            {
                dto.Languages = model.Languages?.Select(l => l.LangCode).ToList();
            });
        CreateMap<ClientInputDto, Client>()
            .ForMember(e => e.Languages, e => e.Ignore())
            .AfterMap((dto, model) =>
            {
                model.Languages = dto.Languages?.Select(l => new ClientLanguage { ClientId = model.Id, LangCode = l }).ToList();
            });
        CreateMap<ClientSubscription, ClientSubscriptionDto>();
        CreateMap<ClientSubscriptionInputDto, ClientSubscription>();
    }
}
