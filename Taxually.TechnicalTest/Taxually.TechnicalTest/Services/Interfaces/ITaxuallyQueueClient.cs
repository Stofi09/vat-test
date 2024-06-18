namespace Taxually.TechnicalTest.Services.Interfaces
{
    public interface ITaxuallyQueueClient
    {
        Task EnqueueAsync(string queueName, byte[] message);
    }
}
