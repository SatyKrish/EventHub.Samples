using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace EventHub.Samples.EPHReceiver
{
    public class SampleEventProcessor : IEventProcessor
    {
        public async Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            // Add any cleanup logic here

            if (reason == CloseReason.Shutdown)
            {
                await context.CheckpointAsync();
            }
            
            Console.WriteLine($"Stopped listening on Parition - {context.Lease.PartitionId}");
        }

        public Task OpenAsync(PartitionContext context)
        {
            Console.WriteLine($"Started listening on Parition - {context.Lease.PartitionId}");

            // Add any startup logic here

            return Task.FromResult<object>(null);
        }

        public async Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            Console.WriteLine($"Received {messages.Count()} messages from Partition - {context.Lease.PartitionId}");

            // Add your processing logic here

            await context.CheckpointAsync().ConfigureAwait(false);
        }
    }
}