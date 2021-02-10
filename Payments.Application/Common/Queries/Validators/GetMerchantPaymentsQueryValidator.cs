using FluentValidation;

namespace Payments.Application.Common.Queries.Validators
{
    public class GetMerchantPaymentsQueryValidator : AbstractValidator<GetMerchantPaymentsQuery>
    {
        public GetMerchantPaymentsQueryValidator()
        {
            RuleFor(x => x.MerchantId)
                .GreaterThan(0);

            RuleFor(x => x.PageNumber)
                .GreaterThan(0);

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .LessThanOrEqualTo(50);
        }
    }
}
