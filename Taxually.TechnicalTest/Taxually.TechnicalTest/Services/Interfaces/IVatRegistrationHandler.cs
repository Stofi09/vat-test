using Taxually.TechnicalTest.Controllers;

namespace Taxually.TechnicalTest.Services.Interfaces
{
    public interface IVatRegistrationHandler
    {
        Task HandleAsync(VatRegistrationRequest request);
    }
}
