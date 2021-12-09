using Azure.Messaging.ServiceBus;
using Sender.Entities;
using Sender.Enuns;

namespace Sender
{
    static class Program 
    {
        private const string queueName =  "sbq-main-queue-eastus2";
        private const string connectionString =  "<Queue-Name>";
        static async Task Main()
        {
            var client = new ServiceBusClient(connectionString);
            var sender = client.CreateSender(queueName);

            IList<Payment> payments = new List<Payment>
            {
                new Payment(10.0, PaymentType.Cash),
                new Payment(22.0, PaymentType.CreditCard),
                new Payment(56.0, PaymentType.DebitCard)
            };

            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

            foreach(var payment in payments)
            {
                if (!messageBatch.TryAddMessage(new ServiceBusMessage(payment.SerializeEntity())))
                    throw new Exception($"Payment is too large to fit in the batch.");
            }

            try
            {
                await sender.SendMessagesAsync(messageBatch);
                Console.WriteLine($"Payments Sent");
            }
            finally
            {
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }
        }       
    }
}