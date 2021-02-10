using MediatR;
using Payments.Application.Common.DTOs;
using Payments.Application.Models;

namespace Payments.Application.Common.Queries
{
    public class GetMerchantPaymentsQuery : IRequest<PaginatedList<PaymentResponse>>
    {
        public GetMerchantPaymentsQuery(int merchantId, int pageNumber, int pageSize)
        {
            MerchantId = merchantId;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int MerchantId { get; private set; }
        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }
    }
}
