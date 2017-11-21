using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messages;

namespace Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            InitiazeQueue("testqueue.Subscriber", "testqueue.Publisher").GetAwaiter().GetResult();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        // InitiazeQueue
        static async Task InitiazeQueue(string EndpointName_Subscriber, string EndpointName_Publisher)
        {
            var endpointConfiguration = new EndpointConfiguration(EndpointName_Subscriber);
            endpointConfiguration.UsePersistence<InMemoryPersistence>();

            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            var routing = transport.Routing();
            routing.RegisterPublisher(
                assembly: typeof(RowMessage).Assembly,
                publisherEndpoint: EndpointName_Publisher);

            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.EnableInstallers();

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}
