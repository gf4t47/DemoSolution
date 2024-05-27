namespace HeadQuarter;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Communication;
using Domain.Message;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
[TestClass]
public class OrderingMessageTests
{
    private static IServiceProvider Setup(ServiceCollection sc, Action<ServiceCollection> postConfig)
    {
        sc.ConfigHeadQuarterCommandHandler();
        sc.ConfigHeadQuarterCommunication();
        postConfig(sc);

        return sc.BuildServiceProvider();
    }
    
    [DataTestMethod]
    public async Task TestReceiveOrderingMessage_Succeed()
    {
        var sp = Setup([], sc =>
        {
            sc.AddTransient<IMessageSender<OrderApproved>, MemoryQueueSender<OrderApproved>>();
        });

        var toSend = new Mock<IMessage<OrderApproved>>();
        toSend.SetupGet(msg => msg.Headers).Returns(new Dictionary<string, string>());
        toSend.SetupGet(msg => msg.Payload).Returns(new OrderApproved());
        
        var sender = sp.GetRequiredService<IMessageSender<OrderApproved>>();
        var ret = await sender.Publish(toSend.Object).ConfigureAwait(false);
        Assert.AreEqual(ResponseType.Ack, ret.Type);
        
        var querier = sp.GetRequiredService<IMessageQuerier<OrderApproved>>();
        Assert.IsNotNull(querier);

        var received = await querier.Receive().ConfigureAwait(false);
        Assert.IsNotNull(received);
    }
}
