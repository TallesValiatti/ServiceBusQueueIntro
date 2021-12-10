using Azure.Messaging.ServiceBus;
using Receiver.Entities;
using Receiver.Enuns;

namespace Receiver
{
    static class Program 
    {
        private const string queueName =  "<Queue-Name>";
        private const string connectionString =  "<Connection-String>";
        static async Task Main()
        {
            var client = new ServiceBusClient(connectionString);
            var receiver = client.CreateProcessor(queueName, new ServiceBusProcessorOptions());

            receiver.ProcessMessageAsync += MessageHandler;
            receiver.ProcessErrorAsync += ErrorHandler;

            await receiver.StartProcessingAsync();

            Console.WriteLine("\nProcessing messages ...\n");
            Console.ReadKey();

            Console.WriteLine("\nStopping the receiver ...");
            await receiver.StopProcessingAsync();
            Console.WriteLine("\nStopped receiving messages");

            await receiver.DisposeAsync();
            await client.DisposeAsync();
        }       

        static async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            var payment = Payment.DeserializeEntity(body);

            Console.WriteLine($"Payment: {payment.PaymentType.ToString()} - ${payment.Value}");

            await args.CompleteMessageAsync(args.Message);
        }

        static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}