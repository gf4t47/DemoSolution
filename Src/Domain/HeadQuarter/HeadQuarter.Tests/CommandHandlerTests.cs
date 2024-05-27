// Copyright (c) Demo.
namespace HeadQuarter;

using System;
using System.Threading.Tasks;
using Communication;
using Domain.Message;
using HeadQuarter.Command;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class CommandHandlerTests
{
    private static IServiceProvider Setup(ServiceCollection sc, Action<ServiceCollection> postConfig)
    {
        sc.ConfigHeadQuarterCommandHandler();
        sc.ConfigHeadQuarterCommunication();
        postConfig(sc);

        return sc.BuildServiceProvider();
    }

    [TestMethod]
    public async Task TestMakeDishes_Succeed()
    {
        var sp = Setup([], sc =>
        {
            sc.AddTransient<IMessageQuerier<DishesScheduled>, MemoryQueueQuerier<DishesScheduled>>();
        });
        Assert.IsNotNull(sp);

        var commandBus = sp.GetRequiredService<HeadQuarterCommandBus>();
        var command = new MakeDishes(new MakeDishedData());
        var ret = await commandBus.Execute(command).ConfigureAwait(false);
        Assert.IsTrue(ret);

        var receiver = sp.GetRequiredService<IMessageQuerier<DishesScheduled>>();
        var msg = await receiver.Receive().ConfigureAwait(false);
        Assert.IsNotNull(msg);
    }

    [TestMethod]
    public async Task TestDeliverDishes_Succeed()
    {
        var sp = Setup([], sc =>
        {
            sc.AddTransient<IMessageQuerier<DeliveryScheduled>, MemoryQueueQuerier<DeliveryScheduled>>();
        });
        Assert.IsNotNull(sp);
        
        var commandBus = sp.GetRequiredService<HeadQuarterCommandBus>();
        var command = new DeliverDishes(new DeliverDishesData());
        var ret = await commandBus.Execute(command).ConfigureAwait(false);
        Assert.IsTrue(ret);

        var receiver = sp.GetRequiredService<IMessageQuerier<DeliveryScheduled>>();
        var msg = await receiver.Receive().ConfigureAwait(false);
        Assert.IsNotNull(msg);
    }
    
}
