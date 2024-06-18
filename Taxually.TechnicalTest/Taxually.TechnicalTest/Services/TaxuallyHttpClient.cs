using Taxually.TechnicalTest.Models;
using Taxually.TechnicalTest.Services.Interfaces;

namespace Taxually.TechnicalTest.Services
{
    public class TaxuallyHttpClient : ITaxuallyHttpClient
    {
        private readonly HttpClient _client;
        private readonly ILogger<TaxuallyHttpClient> _logger;

        public TaxuallyHttpClient(HttpClient client, ILogger<TaxuallyHttpClient> logger)
        {
            _client = client;
            _logger = logger;
        }
        public async Task PostAsync(string url, VatRegistrationRequest request)
        {
            try
            {
                var response = await _client.PostAsJsonAsync(url, request);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, $"HttpRequestException occurred while posting to {url}");
                throw;
            }
        }
    }
}
