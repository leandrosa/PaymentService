using AutoMapper;
using Payments.Application.Common.Commands;
using Payments.Application.Common.DTOs;

namespace Payments.Application.Mapping
{
    public class CommandToDtoMappingProfile : Profile
    {
        public CommandToDtoMappingProfile()
        {
            CreateMap<ProcessPaymentCommand, IssuerRequest>();
        }
    }
}
