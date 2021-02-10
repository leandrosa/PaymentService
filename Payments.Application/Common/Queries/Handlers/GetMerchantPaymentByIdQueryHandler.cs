using AutoMapper;
using MediatR;
using Payments.Application.Common.DTOs;
using Payments.Application.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Payments.Application.Common.Queries.Handlers
{
    class GetMerchantPaymentByIdQueryHandler : IRequestHandler<GetMerchantPaymentByIdQuery, PaymentResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetMerchantPaymentByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<PaymentResponse> Handle(GetMerchantPaymentByIdQuery request, CancellationToken cancellationToken)
        {
            var payment = _context.Payments
                .FirstOrDefault(x => x.Id == request.PaymentId);

            return Task.FromResult(_mapper.Map<PaymentResponse>(payment));
        }
    }
}
