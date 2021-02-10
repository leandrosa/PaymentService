using Payments.Application.Common.DTOs;
using Payments.Application.Interfaces;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Payments.Infrastructure.Gateways
{
    class IssuerApiClient : IIssuerApiClient
    {
        private readonly HttpClient _httpClient;

        public IssuerApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IssuerResponse> ProcessPaymentAsync(IssuerRequest request)
        {
            var jsonUtf8Bytes = JsonSerializer.SerializeToUtf8Bytes(request, new JsonSerializerOptions { WriteIndented = true });
            var response = await _httpClient.PostAsync("v1/payment", new ByteArrayContent(jsonUtf8Bytes));

            return await response.Content.ReadFromJsonAsync<IssuerResponse>(); // We need to map the Issuer status to our own status.
        }

    }
}

