using System.Threading.Tasks;
using NServiceBus;
using Messages;
using NServiceBus.Logging;

public class MyHandler :
    IHandleMessages<RowMessage>
{
    static ILog log = LogManager.GetLogger<MyHandler>();
    public Task Handle(RowMessage message, IMessageHandlerContext context)
    {
        log.Info($"Subscriber has received RowMessage event with Message {message.Message}.");
        return Task.CompletedTask;
    }
}