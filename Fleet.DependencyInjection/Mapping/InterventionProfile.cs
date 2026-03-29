//using AutoMapper;
//using Regira.Fleet.Models.EntityLabels;
//using Regira.Fleet.Models.Interventions;
//using Regira.Fleet.Models.Interventions.Invoices;

//namespace Regira.Fleet.DependencyInjection.Mapping;

//public class InterventionProfile : Profile
//{
//    public InterventionProfile()
//    {
//        CreateMap<Intervention, InterventionDto>();
//        CreateMap<InterventionInputDto, Intervention>()
//            .AfterMap((dto, model) =>
//            {
//                if (string.IsNullOrWhiteSpace(dto.Invoice?.InvoiceNumber) && dto.Invoice?.PriceExcl == null)
//                {
//                    model.Invoice = null;
//                }
//                if (model.Invoice != null)
//                {
//                    model.Invoice.InterventionId = model.Id;
//                }
//            });
//        CreateMap<Invoice, InvoiceDto>();
//        CreateMap<InvoiceInputDto, Invoice>();

//        CreateMap<InterventionLabel, EntityLabelDto>();
//        CreateMap<EntityLabelInputDto, InterventionLabel>();
//    }
//}
