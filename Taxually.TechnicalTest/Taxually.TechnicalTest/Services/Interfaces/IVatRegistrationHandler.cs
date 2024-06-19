using Taxually.TechnicalTest.Controllers;
using Taxually.TechnicalTest.Models;

namespace Taxually.TechnicalTest.Services.Interfaces
{
    public interface IVatRegistrationHandler
    {
        string CountryCode { get; }
        Task HandleAsync(VatRegistrationRequest request);
    }
}
