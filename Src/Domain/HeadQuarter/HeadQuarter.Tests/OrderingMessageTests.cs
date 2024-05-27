namespace HeadQuarter;

using System;
using System.Collections.Generic;
using System.Threading;
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
    
    [TestMethod]
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

    [TestMethod]
    public async Task TestReceiveOrderingMessage_ToDishAndDeliveryMessage()
    {
        var sp = Setup([], sc =>
        {
            sc.AddTransient<IMessageSender<OrderApproved>, MemoryQueueSender<OrderApproved>>();
            sc.AddTransient<IMessageQuerier<DishesScheduled>, MemoryQueueQuerier<DishesScheduled>>();
            sc.AddTransient<IMessageQuerier<DeliveryScheduled>, MemoryQueueQuerier<DeliveryScheduled>>();

            sc.AddSingleton<OrderingListener>();
        });
        
        var toSend = new Mock<IMessage<OrderApproved>>();
        toSend.SetupGet(msg => msg.Headers).Returns(new Dictionary<string, string>());
        toSend.SetupGet(msg => msg.Payload).Returns(new OrderApproved());

        var sender = sp.GetRequiredService<IMessageSender<OrderApproved>>();
        var ret = await sender.Publish(toSend.Object).ConfigureAwait(false);
        Assert.AreEqual(ResponseType.Ack, ret.Type);
        
        var listener = sp.GetRequiredService<OrderingListener>();
        var cancelTokenSource = new CancellationTokenSource();
        await listener.StartAsync(cancelTokenSource.Token).ConfigureAwait(false);

        await Task.Delay(TimeSpan.FromSeconds(3), cancelTokenSource.Token).ConfigureAwait(false);
        
        var dishQuerier = sp.GetRequiredService<IMessageQuerier<DishesScheduled>>();
        var dishMsg = await dishQuerier.Receive().ConfigureAwait(false);
        Assert.IsNotNull(dishMsg);
        
        var deliveryQuerier = sp.GetRequiredService<IMessageQuerier<DeliveryScheduled>>();
        var deliveryMsg = await deliveryQuerier.Receive().ConfigureAwait(false);
        Assert.IsNotNull(deliveryMsg);
        
        await listener.StopAsync(cancelTokenSource.Token).ConfigureAwait(false);
        cancelTokenSource.Cancel();
    }
}
