using System;
using NServiceBus;

namespace Shipping
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            const string endpointName = "Shipping";

            Console.Title = endpointName;

            var endpointConfiguration = new EndpointConfiguration(endpointName);
            ConfigureTransport(endpointConfiguration);

            var persistence = endpointConfiguration.UsePersistence<LearningPersistence>();

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }

        private static void ConfigureTransport(EndpointConfiguration endpointConfiguration)
        {
            //endpointConfiguration.UseTransport<LearningTransport>();
            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            endpointConfiguration.EnableInstallers();
            transport.ConnectionString("host=localhost;username=guest;password=guest");
            transport.UseConventionalRoutingTopology();
            endpointConfiguration.AuditProcessedMessagesTo("personalQueueForAudit");
        }
    }
}
