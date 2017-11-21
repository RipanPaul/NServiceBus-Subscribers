using System.Threading.Tasks;
using NServiceBus;
using Messages;

public class MyHandler :
    IHandleMessages<RowMessage>
{
    public Task Handle(RowMessage message, IMessageHandlerContext context)
    {
        



        return Task.CompletedTask;
    }
}