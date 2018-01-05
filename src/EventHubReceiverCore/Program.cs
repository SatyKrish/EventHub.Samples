using System;
using System.Collections.Generic;
using Microsoft.Azure.EventHubs.Processor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EventHubSamples.EventHubReceiverCore
{
    class Program
    {
        static void Main(string[] args)
        {
            ILoggerFactory loggerFactory = new LoggerFactory()
                .AddEventSourceLogger()
                .AddDebug();

            ILogger logger = loggerFactory.CreateLogger("EventHubLogs");

            var configuration = new ConfigurationBuilder()
                //.AddInMemoryCollection(new Dictionary<string, string>
                //{
                //    ["EventHubName"] = "",
                //    ["ConsumerGroupName"] = "",
                //    ["EventHubConnectionString"] = "",
                //    ["StorageConnectionString"] = "",
                //    ["LeaseContainerName"] = ""
                //})
                .AddJsonFile("AppSettings.json")
                .Build();

            var eventHubPath = configuration["EventHubSettings:EventHubName"];
            var consumerGroupName = configuration["EventHubSettings:ConsumerGroupName"];
            var eventHubConnectionString = configuration["EventHubSettings:EventHubConnectionString"];
            var storageConnectionString = configuration["EventHubSettings:StorageConnectionString"];
            var leaseContainerName = configuration["EventHubSettings:LeaseContainerName"];

            var eph = new EventProcessorHost(eventHubPath, consumerGroupName, eventHubConnectionString, storageConnectionString, leaseContainerName);

            // Default options can be overridden by a new instance of EventProcessorOptions with configurable values.
            var options = EventProcessorOptions.DefaultOptions;

            logger.LogInformation("Starting event processor host.");
            eph.RegisterEventProcessorFactoryAsync(new EventProcessorFactory(logger))
                .ContinueWith(t => logger.LogInformation("Event processor host started."))
                .GetAwaiter()
                .GetResult();

            // This will keep the process running, until manually stopped.
            Console.WriteLine("Press any key to exit...");
            Console.Read();

            logger.LogInformation("Stopping event processor host.");
            eph.UnregisterEventProcessorAsync()
                .ContinueWith(t => logger.LogInformation("Event processor host stopped."))
                .GetAwaiter()
                .GetResult();
        }
    }
}
