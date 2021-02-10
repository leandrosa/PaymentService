using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Payments.Application.Common.DTOs;
using Payments.Application.Extensions;
using Payments.Application.Interfaces;
using Payments.Application.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Application.Common.Queries.Handlers
{
    public class GetMerchantPaymentsQueryHandler : IRequestHandler<GetMerchantPaymentsQuery, PaginatedList<PaymentResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetMerchantPaymentsQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<PaymentResponse>> Handle(GetMerchantPaymentsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Payments
                .AsNoTracking()
                .Where(x => x.MerchantId == request.MerchantId)
                .OrderByDescending(x => x.Created)
                .ProjectTo<PaymentResponse>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
