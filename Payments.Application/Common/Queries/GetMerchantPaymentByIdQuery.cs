using System;
using MediatR;
using Payments.Application.Common.DTOs;

namespace Payments.Application.Common.Queries
{
    public class GetMerchantPaymentByIdQuery : IRequest<PaymentResponse>
    {
        public GetMerchantPaymentByIdQuery(int merchantId, Guid paymentId)
        {
            MerchantId = merchantId;
            PaymentId = paymentId;
        }

        public int MerchantId { get; set; }

        public Guid PaymentId { get; set; }
    }
}
