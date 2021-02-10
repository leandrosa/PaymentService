using Payments.Application.Common.DTOs;
using System.Threading.Tasks;

namespace Payments.Application.Interfaces
{
    public interface IIssuerApiClient
    {
        Task<IssuerResponse> ProcessPaymentAsync(IssuerRequest request);
    }
}
