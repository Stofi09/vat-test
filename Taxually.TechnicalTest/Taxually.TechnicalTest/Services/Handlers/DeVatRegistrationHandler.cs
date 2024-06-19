using System.Text;
using System.Xml.Serialization;
using Taxually.TechnicalTest.Models;
using Taxually.TechnicalTest.Services.Interfaces;

namespace Taxually.TechnicalTest.Services.Handlers
{
    public class DeVatRegistrationHandler : IVatRegistrationHandler
    {
        private readonly ITaxuallyQueueClient _queueClient;
        private readonly ILogger<DeVatRegistrationHandler> _logger;
        private readonly string _queueName;
        public string CountryCode => "DE";
        public DeVatRegistrationHandler(ITaxuallyQueueClient queueClient, IConfiguration configuration, ILogger<DeVatRegistrationHandler> logger)
        {
            _queueClient = queueClient;
            _logger = logger;
            _queueName = configuration["VatRegistration:DeQueueName"] ?? throw new ArgumentNullException("DeQueueName configuration is missing.");
        }

        public async Task HandleAsync(VatRegistrationRequest request)
        {
            try
            {
                using (var stringwriter = new StringWriter())
                {
                    var serializer = new XmlSerializer(typeof(VatRegistrationRequest));
                    serializer.Serialize(stringwriter, request);
                    var xml = stringwriter.ToString();
                    await _queueClient.EnqueueAsync(_queueName, Encoding.UTF8.GetBytes(xml));
                }
             }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error during DE VAT registration for {request.CompanyName}");
                throw;
            }
        }
    }
}
