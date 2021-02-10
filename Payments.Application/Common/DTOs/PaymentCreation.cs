using System;
using Payments.Domain.Enums;

namespace Payments.Application.Common.DTOs
{
    public class PaymentCreation
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
