using AutoMapper;
using Payments.Application.Common.Commands;
using Payments.Domain.Entities;
using Payments.Domain.ValueObjects;

namespace Payments.Application.Mapping
{
    public class CommandToModelMappingProfile : Profile
    {
        public CommandToModelMappingProfile()
        {
            CreateMap<ProcessPaymentCommand, Card>()
                .ConvertUsing(x => new Card(x.CardholderName, x.CardNumber, x.ExpirationDate, x.Cvv));

            CreateMap<ProcessPaymentCommand, Payment>()
                .ConvertUsing((x, _, f) => new Payment(x.MerchantId, f.Mapper.Map<Card>(x), x.Amount, x.Currency));
        }
    }
}
