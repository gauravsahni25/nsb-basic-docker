﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Messages.Events;
using NServiceBus;
using NServiceBus.Logging;

namespace Shipping
{
    public class OrderPlacedHandler : IHandleMessages<OrderPlaced>
    {
        static ILog log = LogManager.GetLogger<OrderPlacedHandler>();

        public Task Handle(OrderPlaced message, IMessageHandlerContext context)
        {
            log.Info($"Received OrderPlaced, OrderId = {message.OrderId} - Should we ship now?");
            
            var orderBilled = new OrderBilled
            {
                OrderId = message.OrderId
            };
            return Task.CompletedTask;
        }
    }
}