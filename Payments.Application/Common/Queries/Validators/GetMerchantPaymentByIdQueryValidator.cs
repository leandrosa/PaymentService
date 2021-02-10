using FluentValidation;

namespace Payments.Application.Common.Queries.Validators
{
    public class GetMerchantPaymentByIdQueryValidator : AbstractValidator<GetMerchantPaymentByIdQuery>
    {
        public GetMerchantPaymentByIdQueryValidator()
        {
            RuleFor(x => x.MerchantId)
                .GreaterThan(0);

            RuleFor(x => x.PaymentId)
                .NotEmpty();
        }
    }
}
