namespace HeadQuarter;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Communication;
using Domain.Message;
using Domain.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ordering.Model;
using Persistence;
[TestClass]
public class OrderingMessageTests
{
    private static IServiceProvider Setup(ServiceCollection sc, Action<ServiceCollection> postConfig)
    {
        sc.ConfigHeadQuarterCommandHandler();
        sc.ConfigHeadQuarterCommunication();
        sc.ConfigHeadQuarterPersistence();
        postConfig(sc);

        return sc.BuildServiceProvider();
    }
    
    private static IEnumerable<(Customer, ICollection<Dishes>, Address)> Submitted
    {
        get
        {
            var address = new Address("street", "LA", "CA", 91100);
            var customer = new Customer(1, "Test", address);
            var dishes = new List<Dishes> { new("Pizza"), new("Noodles")};
            
            yield return (customer, dishes, address);
        }
    }
    
    private static IEnumerable<object[]> TestReceiveOrder
    {
        get
        {
            foreach (var (customer, food, address)  in Submitted)
            {
                yield return [customer, food, address];
            }
        }
    }
    
    [DataTestMethod]
    [DynamicData(nameof(TestReceiveOrder))]
    public async Task TestReceiveOrderingMessage_Succeed(Customer customer, ICollection<Dishes> food, Address address)
    {
        var sp = Setup([], sc =>
        {
            sc.AddTransient<IMessageSender<OrderApproved>, MemoryQueueSender<OrderApproved>>();
        });

        var toSend = new Mock<IMessage<OrderApproved>>();
        toSend.SetupGet(msg => msg.Headers).Returns(new Dictionary<string, string>());
        toSend.SetupGet(msg => msg.Payload).Returns(new OrderApproved(1, customer, food, address));
        
        var sender = sp.GetRequiredService<IMessageSender<OrderApproved>>();
        var ret = await sender.Publish(toSend.Object).ConfigureAwait(false);
        Assert.AreEqual(ResponseType.Ack, ret.Type);
        
        var querier = sp.GetRequiredService<IMessageQuerier<OrderApproved>>();
        Assert.IsNotNull(querier);

        var received = await querier.Receive().ConfigureAwait(false);
        Assert.IsNotNull(received);
        Assert.AreEqual(customer.Id, received.Payload.Customer.Id);
        Assert.AreEqual(address, received.Payload.DeliveryAddress);
        CollectionAssert.AreEqual(food.ToList(), received.Payload.Food.ToList());
    }

    [DataTestMethod]
    [DynamicData(nameof(TestReceiveOrder))]
    public async Task TestReceiveOrderingMessage_ToDishAndDeliveryMessage(Customer customer, ICollection<Dishes> food, Address address)
    {
        var repo = new Mock<IRepository<Order>>();
        repo.Setup(r => r.GetById(It.IsAny<int>())).Returns(Task.FromResult(new Order(1, customer, food, new PaymentInfo(), address)));
        repo.Setup(r => r.Update(It.IsAny<Order>())).Returns(Task.FromResult(true));
        
        var sp = Setup([], sc =>
        {
            sc.AddTransient<IMessageSender<OrderApproved>, MemoryQueueSender<OrderApproved>>();
            sc.AddTransient<IMessageQuerier<DishesScheduled>, MemoryQueueQuerier<DishesScheduled>>();
            sc.AddTransient<IMessageQuerier<DeliveryScheduled>, MemoryQueueQuerier<DeliveryScheduled>>();

            sc.AddSingleton<OrderingListener>();
            sc.AddSingleton<IRepository<Order>>(_ => repo.Object);
        });
        
        var toSend = new Mock<IMessage<OrderApproved>>();
        toSend.SetupGet(msg => msg.Headers).Returns(new Dictionary<string, string>());
        toSend.SetupGet(msg => msg.Payload).Returns(new OrderApproved(1, customer, food, address));
        
        var listener = sp.GetRequiredService<OrderingListener>();
        var cancelTokenSource = new CancellationTokenSource();
        await listener.StartAsync(cancelTokenSource.Token).ConfigureAwait(false);
        
        var sender = sp.GetRequiredService<IMessageSender<OrderApproved>>();
        var ret = await sender.Publish(toSend.Object).ConfigureAwait(false);
        Assert.AreEqual(ResponseType.Ack, ret.Type);

        await Task.Delay(TimeSpan.FromSeconds(2), cancelTokenSource.Token).ConfigureAwait(false);
        
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
