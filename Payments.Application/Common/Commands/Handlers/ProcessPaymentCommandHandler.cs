using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Payments.Application.Common.Commands;
using Payments.Application.Common.DTOs;
using Payments.Application.Interfaces;
using Payments.Domain.Entities;
using Payments.Domain.Enums;

namespace Payments.Application.Commands.Handlers
{
    public class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand, Guid>
    {
        private readonly IIssuerApiClient _issuerApiClient;
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        private readonly INotificationContext _notificationContext;

        public ProcessPaymentCommandHandler(IIssuerApiClient issuerApiClient,
            IApplicationDbContext applicationDbContext,
            IMapper mapper,
            INotificationContext notificationContext)
        {
            _issuerApiClient = issuerApiClient;
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
            _notificationContext = notificationContext;
        }

        public async Task<Guid> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
        {
            if (_notificationContext.HasNotifications) return Guid.Empty;

            var payment = _mapper.Map<Payment>(request);

            if (payment.HasNotifications)
            {
                _notificationContext.AddNotifications(payment.Notifications);
                return Guid.Empty;
            }

            var issuerRequest = _mapper.Map<IssuerRequest>(request);

            var paymentProcessement = await _issuerApiClient.ProcessPaymentAsync(issuerRequest);

            payment.SetPaymentStatus((PaymentStatus)paymentProcessement.ProcessingStatus);
            payment.SetIssuerProcessingId(paymentProcessement.ProcessingId);

            if (payment.HasNotifications)
            {
                _notificationContext.AddNotifications(payment.Notifications);
                return Guid.Empty;
            }

            _applicationDbContext.Payments.Add(payment);
            await _applicationDbContext.SaveChangesAsync(CancellationToken.None);

            return payment.Id;
        }
    }
}
