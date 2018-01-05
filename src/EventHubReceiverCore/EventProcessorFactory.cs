using Microsoft.Azure.EventHubs.Processor;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventHubSamples.EventHubReceiverCore
{
    public class EventProcessorFactory : IEventProcessorFactory
    {
        readonly ILogger logger;

        public EventProcessorFactory(ILogger logger)
        {
            this.logger = logger;
        }

        public IEventProcessor CreateEventProcessor(PartitionContext context)
        {
            return new SampleEventProcessor(logger);
        }
    }
}
