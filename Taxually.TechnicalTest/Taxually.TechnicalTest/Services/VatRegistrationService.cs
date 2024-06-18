using Microsoft.AspNetCore.Mvc;
using Taxually.TechnicalTest.Models;
using Taxually.TechnicalTest.Services.Interfaces;

namespace Taxually.TechnicalTest.Services
{
    public class VatRegistrationService
    {
        private readonly Dictionary<string, IVatRegistrationHandler> _handlers;
        public VatRegistrationService(IEnumerable<IVatRegistrationHandler> handlers)
        {
            _handlers = handlers.ToDictionary(h => h.GetType().Name.Replace("VatRegistrationHandler", ""), h => h);
        }

        public async Task<ActionResult> RegisterVatAsync(VatRegistrationRequest request)
        {
            if (_handlers.TryGetValue(request.Country, out var handler))
            {
                await handler.HandleAsync(request);
                return new OkResult();
            }

            return new BadRequestObjectResult("Country not supported");
        }
    }
}
