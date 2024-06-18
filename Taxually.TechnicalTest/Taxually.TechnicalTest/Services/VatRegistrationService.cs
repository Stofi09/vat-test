using Microsoft.AspNetCore.Mvc;
using Taxually.TechnicalTest.Models;
using Taxually.TechnicalTest.Services.Interfaces;

namespace Taxually.TechnicalTest.Services
{
    public class VatRegistrationService
    {
        private readonly Dictionary<string, IVatRegistrationHandler> _handlers;
        private readonly ILogger<VatRegistrationService> _logger;

        public VatRegistrationService(IEnumerable<IVatRegistrationHandler> handlers, ILogger<VatRegistrationService> logger)
        {
            _handlers = handlers.ToDictionary(h => h.GetType().Name.Replace("VatRegistrationHandler", ""), h => h);
            _logger = logger;
        }

        public async Task<ActionResult> RegisterVatAsync(VatRegistrationRequest request)
        {
            if (_handlers.TryGetValue(request.Country, out var handler))
            {
                try
                {
                    await handler.HandleAsync(request);
                    return new OkResult();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during VAT registration for {Country} - {CompanyName}", request.Country, request.CompanyName);
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }

            _logger.LogWarning("VAT registration failed: Unsupported country {Country}", request.Country);
            return new BadRequestObjectResult("Country not supported");
        }
    }
}
