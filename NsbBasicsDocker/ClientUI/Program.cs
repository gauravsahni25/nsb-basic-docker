﻿using System;
using System.Threading.Tasks;
using Messages.Commands;
using NServiceBus;
using NServiceBus.Logging;

namespace ClientUI
{
    class Program
    {
        static ILog log = LogManager.GetLogger<Program>();
        static async Task Main(string[] args)
        {
            const string endpointName = "ClientUI";

            Console.Title = endpointName;
            
            var endpointConfiguration = ConfigureEndpointAndRouting(endpointName);

            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);
            await RunLoop(endpointInstance)
                .ConfigureAwait(false);

            
            await endpointInstance.Stop()
                .ConfigureAwait(false);

        }

        private static EndpointConfiguration ConfigureEndpointAndRouting(string endpointName)
        {
            var endpointConfiguration = new EndpointConfiguration(endpointName);
            var transport = endpointConfiguration.UseTransport<LearningTransport>();
            
            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(PlaceOrder), "Sales");
            
            return endpointConfiguration;
        }

        private static async Task RunLoop(IEndpointInstance endpointInstance)
        {
            while (true)
            {
                log.Info("Press 'P' to place an order, or 'Q' to quit.");
                var key = Console.ReadKey();
                Console.WriteLine();

                switch (key.Key)
                {
                    case ConsoleKey.P:
                        // Instantiate the command
                        var command = new PlaceOrder
                        {
                            OrderId = Guid.NewGuid().ToString()
                        };

                        // Send the command to the local endpoint
                        log.Info($"Sending PlaceOrder command, OrderId = {command.OrderId}");
                        await endpointInstance.Send(command)
                            .ConfigureAwait(false);

                        break;

                    case ConsoleKey.Q:
                        return;

                    default:
                        log.Info("Unknown input. Please try again.");
                        break;
                }
            }
        }
    }
}