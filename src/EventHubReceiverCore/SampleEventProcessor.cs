using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using Microsoft.Extensions.Logging;

namespace EventHubSamples.EventHubReceiverCore
{
    public class SampleEventProcessor : IEventProcessor
    {
        readonly ILogger logger;

        public SampleEventProcessor(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            // Add any cleanup logic here

            if (reason == CloseReason.Shutdown)
            {
                await context.CheckpointAsync();
            }

            this.logger.LogInformation($"Stopped listening on Parition - {context.PartitionId}");
        }

        public Task OpenAsync(PartitionContext context)
        {
            // Add any startup logic here

            this.logger.LogInformation($"Started listening on Parition - {context.PartitionId}");

            return Task.FromResult<object>(null);
        }

        public Task ProcessErrorAsync(PartitionContext context, Exception error)
        {
            this.logger.LogError(error, $"Encountered error while receiving messages from Parition - {context.PartitionId}");

            // Add any error handling logic here

            return Task.FromResult<object>(null);
        }

        public async Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            this.logger.LogInformation($"Received {messages.Count()} messages from Partition - {context.PartitionId}");

            // Add your processing logic here
            foreach (var message in messages)
            {
                this.logger.LogInformation(Encoding.UTF8.GetString(message.Body.Array, message.Body.Offset, message.Body.Count));
            }

            await context.CheckpointAsync().ConfigureAwait(false);
        }
    }
}