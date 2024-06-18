using System.Text;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;
using Taxually.TechnicalTest.Models;
using Taxually.TechnicalTest.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Taxually.TechnicalTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VatRegistrationController : ControllerBase
    {
        private readonly VatRegistrationService _vatRegistrationService;
        private readonly ILogger<VatRegistrationController> _logger;

        public VatRegistrationController(VatRegistrationService vatRegistrationService, ILogger<VatRegistrationController> logger)
        {
            _vatRegistrationService = vatRegistrationService;
            _logger = logger;
        }
        /// <summary>
        /// Registers a company for a VAT number in a given country
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] VatRegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                return await _vatRegistrationService.RegisterVatAsync(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing VAT registration for {request.Country} - {request.CompanyName}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }
    }
}
