using System.Text;
using System.Xml.Serialization;
using Taxually.TechnicalTest.Models;
using Taxually.TechnicalTest.Services.Interfaces;

namespace Taxually.TechnicalTest.Services.Handlers
{
    public class DeVatRegistrationHandler : IVatRegistrationHandler
    {
        private readonly ITaxuallyQueueClient _queueClient;

        public DeVatRegistrationHandler(ITaxuallyQueueClient queueClient)
        {
            _queueClient = queueClient;
        }

        //Add try catch
        public async Task HandleAsync(VatRegistrationRequest request)
        {
            using (var stringwriter = new StringWriter())
            {
                var serializer = new XmlSerializer(typeof(VatRegistrationRequest));
                serializer.Serialize(stringwriter, request);
                var xml = stringwriter.ToString();
                await _queueClient.EnqueueAsync("vat-registration-xml", Encoding.UTF8.GetBytes(xml));
            }
        }
    }
}
