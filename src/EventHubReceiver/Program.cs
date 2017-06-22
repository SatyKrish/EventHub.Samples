using System;
using System.Configuration;
using Microsoft.ServiceBus.Messaging;

namespace EventHub.Samples.EPHReceiver
{
    class Program
    {
        static void Main(string[] args)
        {
            var eventHubPath = ConfigurationManager.AppSettings["EventHubName"];
            var consumerGroupName = ConfigurationManager.AppSettings["ConsumerGroupName"];
            var eventHubConnectionString = ConfigurationManager.AppSettings["EventHubConnectionString"];
            var storageConnectionString = ConfigurationManager.AppSettings["StorageConnectionString"];
            
            var eph = new EventProcessorHost(eventHubPath, consumerGroupName, eventHubConnectionString, storageConnectionString);

            // Default options can be overridden by a new instance of EventProcessorOptions with configurable values.
            var options = EventProcessorOptions.DefaultOptions;
            options.ExceptionReceived += EventProcessor_ExceptionReceived;

            Console.WriteLine("Starting event processor host.");
            eph.RegisterEventProcessorAsync<SampleEventProcessor>(EventProcessorOptions.DefaultOptions)
                .ContinueWith(t => Console.WriteLine("Event processor host started.")).GetAwaiter().GetResult();

            // This will keep the process running, until manually stopped.
            Console.WriteLine("Press any key to exit...");
            Console.Read();

            Console.WriteLine("Stopping event processor host.");
            eph.UnregisterEventProcessorAsync()
                .ContinueWith(t => Console.WriteLine("Event processor host stopped.")).GetAwaiter().GetResult();
        }

        private static void EventProcessor_ExceptionReceived(object sender, ExceptionReceivedEventArgs e)
        {
            // Handle event processor failures here.

            Console.WriteLine($"Error occured in event processor. \n {e.Exception}");
        }
    }
}
