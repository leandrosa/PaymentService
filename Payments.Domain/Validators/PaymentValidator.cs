using FluentValidation;
using Payments.Domain.Validators;

namespace Payments.Domain.Entities.Validators
{
    public class PaymentValidator : AbstractValidator<Payment>
    {
        public PaymentValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.MerchantId)
                .NotEmpty();

            RuleFor(x => x.Currency)
                .IsInEnum();

            RuleFor(x => x.Amount)
                .GreaterThan(0);

            RuleFor(x => x.Card)
                .NotNull()
                .SetValidator(new CardValidator());
        }
    }
}
