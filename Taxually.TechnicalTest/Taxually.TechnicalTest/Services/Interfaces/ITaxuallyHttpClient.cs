using Taxually.TechnicalTest.Controllers;

namespace Taxually.TechnicalTest.Services.Interfaces
{
    public interface ITaxuallyHttpClient
    {
        Task PostAsync(string url, VatRegistrationRequest request);
    }
}
