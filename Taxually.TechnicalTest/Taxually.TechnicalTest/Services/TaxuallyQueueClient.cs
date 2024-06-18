using Taxually.TechnicalTest.Services.Interfaces;

namespace Taxually.TechnicalTest.Services
{
    public class TaxuallyQueueClient : ITaxuallyQueueClient
    {
        public async Task EnqueueAsync(string queueName, byte[] message)
        {
            await Task.CompletedTask;
        }
    }
}
