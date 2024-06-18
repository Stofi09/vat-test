using Taxually.TechnicalTest.Models;
using Taxually.TechnicalTest.Services.Interfaces;

namespace Taxually.TechnicalTest.Services.Handlers
{
    public class UkVatRegistrationHandler : IVatRegistrationHandler
    {
        private readonly ITaxuallyHttpClient _httpClient;

        public UkVatRegistrationHandler(ITaxuallyHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        //Add try catch, get the link from config
        public async Task HandleAsync(VatRegistrationRequest request)
        {
            await _httpClient.PostAsync("https://api.uktax.gov.uk", request);
        }
    }
}
