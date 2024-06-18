using Taxually.TechnicalTest.Controllers;
using Taxually.TechnicalTest.Models;

namespace Taxually.TechnicalTest.Services.Interfaces
{
    public interface ITaxuallyHttpClient
    {
        Task PostAsync(string url, VatRegistrationRequest request);
    }
}
