using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Regira.Fleet.Core.Constants;
using Regira.Fleet.Identity.Entities.Clients;
using Regira.Fleet.Identity.Entities.Clients.Subscriptions;
using Regira.Fleet.Identity.Entities.Users;
using Regira.Fleet.Identity.Entities.Users.Claims;

namespace Regira.Fleet.Identity.Entities;

public class IdentityProfile : Profile
{
    public IdentityProfile()
    {
        CreateMap<FleetUser, FleetUserModel>().ReverseMap();
        CreateMap<ClientUserClaim, ClientUserClaimDto>().ReverseMap();
        CreateMap<FleetUserModel, FleetUserDto>()
            .ForMember(e => e.UserClaims, e => e.Ignore())
            .AfterMap((model, dto, ctx) =>
            {
                dto.Clients = ctx.Mapper.Map<List<ClientDto>>(model.Clients);
                dto.UserClaims = model.UserClaims?.Select(x => new UserClaimDto { Id = x.Id, ClaimType = x.ClaimType!, ClaimValue = x.ClaimValue }).ToList();
                dto.GivenName = dto.UserClaims?.FirstOrDefault(c => c.ClaimType == FleetClaimTypes.GivenName)?.ClaimValue;
                dto.LastName = dto.UserClaims?.FirstOrDefault(c => c.ClaimType == FleetClaimTypes.LastName)?.ClaimValue;
            });
        CreateMap<FleetUserInputDto, FleetUserModel>()
            .ForMember(e => e.UserClaims, e => e.Ignore())
            .AfterMap((dto, model, ctx) =>
            {
                if (dto.UserClaims != null)
                {
                    var userClaims = dto.UserClaims.Select(x => new IdentityUserClaim<string> { Id = x.Id, ClaimType = x.ClaimType!, ClaimValue = x.ClaimValue }).ToList();
                    if (!string.IsNullOrWhiteSpace(dto.GivenName))
                    {
                        var claim = userClaims.FirstOrDefault(c => c.ClaimType == FleetClaimTypes.GivenName);
                        if (claim != null)
                        {
                            claim.ClaimValue = dto.GivenName;
                        }
                        else
                        {
                            userClaims.Add(new IdentityUserClaim<string> { ClaimType = FleetClaimTypes.GivenName, ClaimValue = dto.GivenName });
                        }
                    }
                    if (!string.IsNullOrWhiteSpace(dto.LastName))
                    {
                        var claim = userClaims.FirstOrDefault(c => c.ClaimType == FleetClaimTypes.LastName);
                        if (claim != null)
                        {
                            claim.ClaimValue = dto.LastName;
                        }
                        else
                        {
                            userClaims.Add(new IdentityUserClaim<string> { ClaimType = FleetClaimTypes.LastName, ClaimValue = dto.LastName });
                        }
                    }
                    model.UserClaims = userClaims;
                }
                if (dto.ClientClaims != null)
                {
                    model.ClientClaims = ctx.Mapper.Map<List<ClientUserClaim>>(dto.ClientClaims);
                }
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
