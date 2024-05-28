﻿// Copyright (c) Demo.
namespace HeadQuarter;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Communication;
using Domain.Message;
using Domain.Model;
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
    public async Task TestMakeDishes_Succeed(Customer customer, ICollection<Dishes> food, Address address)
    {
        var sp = Setup([], sc =>
        {
            sc.AddTransient<IMessageQuerier<DishesScheduled>, MemoryQueueQuerier<DishesScheduled>>();
        });
        Assert.IsNotNull(sp);

        var commandBus = sp.GetRequiredService<HeadQuarterCommandBus>();
        var command = new MakeDishes(new MakeDishedData(customer, food));
        var ret = await commandBus.Execute(command).ConfigureAwait(false);
        Assert.IsTrue(ret);

        var receiver = sp.GetRequiredService<IMessageQuerier<DishesScheduled>>();
        var msg = await receiver.Receive().ConfigureAwait(false);
        Assert.IsNotNull(msg);
    }

    [DataTestMethod]
    [DynamicData(nameof(TestReceiveOrder))]
    public async Task TestDeliverDishes_Succeed(Customer customer, ICollection<Dishes> food, Address address)
    {
        var sp = Setup([], sc =>
        {
            sc.AddTransient<IMessageQuerier<DeliveryScheduled>, MemoryQueueQuerier<DeliveryScheduled>>();
        });
        Assert.IsNotNull(sp);
        
        var commandBus = sp.GetRequiredService<HeadQuarterCommandBus>();
        var command = new DeliverDishes(new DeliverDishesData(customer, address));
        var ret = await commandBus.Execute(command).ConfigureAwait(false);
        Assert.IsTrue(ret);

        var receiver = sp.GetRequiredService<IMessageQuerier<DeliveryScheduled>>();
        var msg = await receiver.Receive().ConfigureAwait(false);
        Assert.IsNotNull(msg);
    }
    
}
