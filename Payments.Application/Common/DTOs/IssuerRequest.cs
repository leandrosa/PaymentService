using System;

namespace Payments.Application.Common.DTOs
{
    public class IssuerRequest
    {
        public string CardholderName { get; set; }

        public string CardNumber { get; set; }

        public DateTime ExpirationDate { get; set; }

        public decimal Amount { get; set; }

        public int Currency { get; set; }

        public string Cvv { get; set; }
    }
}
