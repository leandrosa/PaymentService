using Payments.Domain.Enums;
using System;

namespace Payments.Application.Common.DTOs
{
    public class PaymentResponse
    {
        public Guid PaymentId { get; set; }

        public int MerchantId { get; set; }

        public string CardholderName { get; set; }

        public string CardNumber { get; set; }

        public DateTime ExpirationDate { get; set; }

        public string Cvv { get; set; }

        public decimal Amount { get; set; }

        public Currency Currency { get; set; }

        public Guid IssuerProcessingId { get; set; }

        public PaymentStatus PaymentStatus { get; set; }
    }
}
