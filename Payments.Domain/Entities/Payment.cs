using System;
using Payments.Domain.Common;
using Payments.Domain.Entities.Validators;
using Payments.Domain.Enums;
using Payments.Domain.Interfaces;
using Payments.Domain.ValueObjects;

namespace Payments.Domain.Entities
{
    public class Payment : Entity, IAggregateRoot
    {
        public Guid Id { get; private set; }

        public int MerchantId { get; private set; }

        public Card Card { get; private set; }

        public decimal Amount { get; private set; }

        public Currency Currency { get; private set; }

        public PaymentStatus PaymentStatus { get; private set; }

        public Guid IssuerProcessingId { get; private set; }

        // Empty constructor for EF
        protected Payment() { }

        public Payment(int merchantId, Card card, decimal amount, Currency currency)
        {
            Id = Guid.NewGuid();
            MerchantId = merchantId;
            Card = card;
            Amount = amount;
            Currency = currency;

            Validate(this, new PaymentValidator());
        }

        public void SetPaymentStatus(PaymentStatus paymentStatus)
        {
            if (!Enum.IsDefined(typeof(PaymentStatus), paymentStatus))
            {
                AddNotification("PaymentStatusValidation", "Invalid payment status.");
                return;
            }

            PaymentStatus = paymentStatus;
        }

        public void SetIssuerProcessingId(Guid issuerProcessingId)
        {
            if (issuerProcessingId == Guid.Empty)
            {
                AddNotification("NotEmptyValidator", "Empty issuer processing identifier.");
                return;
            }

            IssuerProcessingId = issuerProcessingId;
        }
    }
}
