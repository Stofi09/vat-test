using System.Text;
using Taxually.TechnicalTest.Models;
using Taxually.TechnicalTest.Services.Interfaces;

namespace Taxually.TechnicalTest.Services.Handlers
{
    public class FrVatRegistrationHandler : IVatRegistrationHandler
    {
        private readonly ITaxuallyQueueClient _queueClient;
        private readonly ILogger<FrVatRegistrationHandler> _logger;
        private readonly string _queueName;
        public string CountryCode => "FR";
        public FrVatRegistrationHandler(ITaxuallyQueueClient queueClient, IConfiguration configuration, ILogger<FrVatRegistrationHandler> logger)
        {
            _queueClient = queueClient;
            _logger = logger;
            _queueName = configuration["VatRegistration:FrQueueName"] ?? throw new ArgumentNullException("FrQueueName configuration is missing.");
        }

        public async Task HandleAsync(VatRegistrationRequest request)
        {
            try
            {
                var csvBuilder = new StringBuilder();
                csvBuilder.AppendLine("CompanyName,CompanyId");
                csvBuilder.AppendLine($"{request.CompanyName},{request.CompanyId}");
                var csv = Encoding.UTF8.GetBytes(csvBuilder.ToString());
                await _queueClient.EnqueueAsync(_queueName, csv);
             }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error during FR VAT registration for {request.CompanyName}");
                throw;
            }
        }
    }
}
