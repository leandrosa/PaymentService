using AutoMapper;
using Payments.Application.Common.DTOs;
using Payments.Domain.Entities;

namespace Payments.Application.Mapping
{
    class ModelToDtoMappingProfile : Profile
    {
        public ModelToDtoMappingProfile()
        {
            CreateMap<Payment, PaymentResponse>()
                .ForMember(x => x.CardholderName, opt => opt.MapFrom(origin => origin.Card.CardholderName))
                .ForMember(x => x.CardNumber, opt => opt.MapFrom(origin => origin.Card.MaskedCardNumber))
                .ForMember(x => x.Cvv, opt => opt.MapFrom(origin => origin.Card.Cvv))
                .ForMember(x => x.ExpirationDate, opt => opt.MapFrom(origin => origin.Card.ExpirationDate))
                .ForMember(x => x.PaymentId, opt => opt.MapFrom(origin => origin.Id));
        }
    }
}
