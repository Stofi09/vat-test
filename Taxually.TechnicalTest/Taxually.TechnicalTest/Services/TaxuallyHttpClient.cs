using Taxually.TechnicalTest.Models;
using Taxually.TechnicalTest.Services.Interfaces;

namespace Taxually.TechnicalTest.Services
{
    public class TaxuallyHttpClient : ITaxuallyHttpClient
    {
        public async Task PostAsync(string url, VatRegistrationRequest request)
        {
            await Task.CompletedTask;
        }
    }
}
