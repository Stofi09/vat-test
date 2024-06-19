using Microsoft.Extensions.Logging;
using Taxually.TechnicalTest.Models;
using Taxually.TechnicalTest.Services.Interfaces;

namespace Taxually.TechnicalTest.Services.Handlers
{
    public class UkVatRegistrationHandler : IVatRegistrationHandler
    {
        private readonly ITaxuallyHttpClient _httpClient;
        private readonly ILogger<UkVatRegistrationHandler> _logger;
        private readonly string _apiUrl;
        public string CountryCode => "GB";
        public UkVatRegistrationHandler(ITaxuallyHttpClient httpClient, IConfiguration configuration, ILogger<UkVatRegistrationHandler> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _apiUrl = configuration["VatRegistration:UkApiUrl"] ?? throw new ArgumentNullException("UkApiUrl configuration is missing.");
        }

        public async Task HandleAsync(VatRegistrationRequest request)
        {
            try
            {
                await _httpClient.PostAsync(_apiUrl, request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error during UK VAT registration for {request.CompanyName}");
                throw; 
            }
        }
    }
}
