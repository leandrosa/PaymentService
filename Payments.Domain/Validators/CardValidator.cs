using System;
using FluentValidation;
using Payments.Domain.ValueObjects;

namespace Payments.Domain.Validators
{
    public class CardValidator : AbstractValidator<Card>
    {
        public CardValidator()
        {
            RuleFor(x => x.CardholderName)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.CardNumber)
                .CreditCard();

            RuleFor(x => x.Cvv)
                .Matches(@"^\d{3}$");

            RuleFor(x => x.ExpirationDate)
                .NotEmpty()
                .GreaterThan(DateTime.Now)
                .LessThan(DateTime.Now.AddYears(6));
        }
    }
}
