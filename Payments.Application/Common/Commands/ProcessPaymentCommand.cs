using MediatR;
using Payments.Domain.Enums;
using System;

namespace Payments.Application.Common.Commands
{
    public class ProcessPaymentCommand : IRequest<Guid>
    {
        public int MerchantId { get; set; }

        public string CardholderName { get; set; }

        public string CardNumber { get; set; }

        public DateTime ExpirationDate { get; set; }

        public decimal Amount { get; set; }

        public Currency Currency { get; set; }

        public string Cvv { get; set; }
    }
}
