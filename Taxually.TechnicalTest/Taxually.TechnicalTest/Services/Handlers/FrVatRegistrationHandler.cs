using System.Text;
using Taxually.TechnicalTest.Models;
using Taxually.TechnicalTest.Services.Interfaces;

namespace Taxually.TechnicalTest.Services.Handlers
{
    public class FrVatRegistrationHandler : IVatRegistrationHandler
    {
        private readonly ITaxuallyQueueClient _queueClient;

        public FrVatRegistrationHandler(ITaxuallyQueueClient queueClient)
        {
            _queueClient = queueClient;
        }

        //Add try catch and config
        public async Task HandleAsync(VatRegistrationRequest request)
        {
            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("CompanyName,CompanyId");
            csvBuilder.AppendLine($"{request.CompanyName},{request.CompanyId}");
            var csv = Encoding.UTF8.GetBytes(csvBuilder.ToString());
            await _queueClient.EnqueueAsync("vat-registration-csv", csv);
        }
    }
}
