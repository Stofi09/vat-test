using Taxually.TechnicalTest.Controllers;
using Taxually.TechnicalTest.Models;

namespace Taxually.TechnicalTest.Services.Interfaces
{
    public interface IVatRegistrationHandler
    {
        Task HandleAsync(VatRegistrationRequest request);
    }
}
