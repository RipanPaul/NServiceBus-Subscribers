using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messages;
using NServiceBus.Persistence.Sql;
using System.Data.SqlClient;

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
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.EnableInstallers();

            // Persistence
            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            var connection = @"Data Source=RIPANPC\SqlExpress;Initial Catalog=TestMsmqDB;UID=sa;Password=Password123;Integrated Security=True";
            persistence.SqlVariant(SqlVariant.MsSqlServer);
            persistence.ConnectionBuilder(
                connectionBuilder: () =>
                {
                    return new SqlConnection(connection);
                });

            var subscriptions = persistence.SubscriptionSettings();
            subscriptions.CacheFor(TimeSpan.FromDays(1));

            // Transport
            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            var routing = transport.Routing();
            routing.RegisterPublisher(
                assembly: typeof(RowMessage).Assembly,
                publisherEndpoint: EndpointName_Publisher);


            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}
