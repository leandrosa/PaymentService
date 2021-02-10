using System;
using System.Collections.Generic;
using Payments.Domain.Common;

namespace Payments.Domain.ValueObjects
{
    public class Card : ValueObject
    {
        public string CardholderName { get; private set; }

        public string CardNumber { get; private set; }

        public DateTime ExpirationDate { get; private set; }

        public string Cvv { get; private set; }

        public string MaskedCardNumber
        {
            get
            {
                return CardNumber == null 
                    ? null 
                    : string.Concat(
                        "".PadLeft(12, '*'),
                        CardNumber[^4..]);
            }
        }

        public Card(string cardholderName, string cardNumber, DateTime expirationDate, string cvv)
        {
            CardholderName = cardholderName;
            CardNumber = cardNumber;
            ExpirationDate = expirationDate;
            Cvv = cvv;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return CardholderName;
            yield return CardNumber;
            yield return ExpirationDate;
            yield return Cvv;
        }
    }
}
