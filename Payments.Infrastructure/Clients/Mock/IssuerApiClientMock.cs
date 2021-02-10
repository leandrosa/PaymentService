using Payments.Application.Common.DTOs;
using Payments.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace Payments.Infrastructure.Gateways.Mock
{
    class IssuerApiClientMock : IIssuerApiClient
    {
        public Task<IssuerResponse> ProcessPaymentAsync(IssuerRequest request)
        {
            var response = new IssuerResponse
            {
                ProcessingId = Guid.NewGuid(),
                ProcessingStatus = new Random().Next(1, 5)
            };

            return Task.FromResult(response);
        }
    }
}
