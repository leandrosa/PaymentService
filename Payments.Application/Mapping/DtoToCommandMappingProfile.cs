using AutoMapper;
using Payments.Application.Common.Commands;
using Payments.Application.Common.DTOs;

namespace Payments.Application.Mapping
{
    public class DtoToCommandMappingProfile : Profile
    {
        public DtoToCommandMappingProfile()
        {
            CreateMap<PaymentCreation, ProcessPaymentCommand>();
        }
    }
}
