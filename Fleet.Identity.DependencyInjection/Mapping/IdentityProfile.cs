//using AutoMapper;
//using Microsoft.AspNetCore.Identity;
//using Regira.Fleet.Identity.Models.Tenants;
//using Regira.Fleet.Identity.Models.Tenants.Subscriptions;
//using Regira.Fleet.Identity.Models.Users;
//using Regira.Fleet.Identity.Models.Users.Claims;

//namespace Regira.Fleet.Identity.DependencyInjection.Mapping;

//public class IdentityProfile : Profile
//{
//    public IdentityProfile()
//    {
//        CreateMap<FleetUser, FleetUserModel>()
//            .AfterMap((item, model) =>
//            {
//                model.IsEmailConfirmed = item.EmailConfirmed;
//            })
//            .ReverseMap();
//        CreateMap<TenantUserClaim, TenantUserClaimDto>().ReverseMap();
//        CreateMap<FleetUserModel, FleetUserDto>()
//            .ForMember(e => e.UserClaims, e => e.Ignore())
//            .AfterMap((model, dto, ctx) =>
//            {
//                dto.Tenants = ctx.Mapper.Map<List<TenantDto>>(model.Tenants);
//                dto.UserClaims = model.UserClaims?.Select(x => new UserClaimDto { Id = x.Id, ClaimType = x.ClaimType!, ClaimValue = x.ClaimValue }).ToList();
//                //dto.GivenName = dto.UserClaims?.FirstOrDefault(c => c.ClaimType == FleetClaimTypes.GivenName)?.ClaimValue;
//                //dto.LastName = dto.UserClaims?.FirstOrDefault(c => c.ClaimType == FleetClaimTypes.LastName)?.ClaimValue;
//            });
//        CreateMap<FleetUserInputDto, FleetUserModel>()
//            .ForMember(e => e.UserClaims, e => e.Ignore())
//            .AfterMap((dto, model, ctx) =>
//            {
//                if (string.IsNullOrWhiteSpace(model.UserName))
//                {
//                    model.UserName = dto.Email;
//                }
//                if (dto.UserClaims != null)
//                {
//                    var userClaims = dto.UserClaims.Select(x => new IdentityUserClaim<string> { Id = x.Id, ClaimType = x.ClaimType, ClaimValue = x.ClaimValue }).ToList();
//                    //if (!string.IsNullOrWhiteSpace(dto.GivenName))
//                    //{
//                    //    var claim = userClaims.FirstOrDefault(c => c.ClaimType == FleetClaimTypes.GivenName);
//                    //    if (claim != null)
//                    //    {
//                    //        claim.ClaimValue = dto.GivenName;
//                    //    }
//                    //    else
//                    //    {
//                    //        userClaims.Add(new IdentityUserClaim<string> { ClaimType = FleetClaimTypes.GivenName, ClaimValue = dto.GivenName });
//                    //    }
//                    //}
//                    //if (!string.IsNullOrWhiteSpace(dto.LastName))
//                    //{
//                    //    var claim = userClaims.FirstOrDefault(c => c.ClaimType == FleetClaimTypes.LastName);
//                    //    if (claim != null)
//                    //    {
//                    //        claim.ClaimValue = dto.LastName;
//                    //    }
//                    //    else
//                    //    {
//                    //        userClaims.Add(new IdentityUserClaim<string> { ClaimType = FleetClaimTypes.LastName, ClaimValue = dto.LastName });
//                    //    }
//                    //}
//                    model.UserClaims = userClaims;
//                }
//                if (dto.TenantClaims != null)
//                {
//                    model.TenantClaims = ctx.Mapper.Map<List<TenantUserClaim>>(dto.TenantClaims);
//                }
//            });

//        CreateMap<Tenant, TenantDto>()
//            .ForMember(e => e.Languages, e => e.Ignore())
//            .AfterMap((model, dto) =>
//            {
//                dto.Languages = model.Languages?.Select(l => l.LangCode).ToList();
//            });
//        CreateMap<TenantInputDto, Tenant>()
//            .ForMember(e => e.Languages, e => e.Ignore())
//            .AfterMap((dto, model) =>
//            {
//                model.Languages = dto.Languages?.Select(l => new TenantLanguage { TenantId = model.Id, LangCode = l }).ToList();
//            });
//        CreateMap<TenantSubscription, TenantSubscriptionDto>();
//        CreateMap<TenantSubscriptionInputDto, TenantSubscription>();
//    }
//}
