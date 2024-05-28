// Copyright (c) Demo.
namespace Ordering;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;using Domain;
using Domain.Model;
using Microsoft.Extensions.Hosting;
using Ordering.Command;
using Ordering.Model;
public class OrderingHostService(OrderingCommandBus commandBus) : BackgroundLongRunner(TimeSpan.FromSeconds(10)), IHostedService
{
    private OrderingCommandBus CommandBus { get; } = commandBus;

    /// <summary>
    /// Mock orders for console demo purpose
    /// </summary>
    private static IEnumerable<SubmitOrder> PredefinedCommands
    {
        get
        {
            {
                var address = new Address("long long beach", "LA", "CA", 91100);
                var customer = new Customer(11, "Test1", address);
                var dishes = new List<Dishes> { new("Pizza"), new("Noodles")};
                var order = new SubmitData(customer, dishes, new PaymentInfo()) { Destination = address };
                yield return new SubmitOrder(order);
            }

            {
                var address = new Address("high high sky", "Top", "FL", 93245);
                var customer = new Customer(12, "Test2", address);
                var dishes = new List<Dishes> { new("Ramen"), new("Burger")};
                var order = new SubmitData(customer, dishes, new PaymentInfo()) { Destination = address };
                yield return new SubmitOrder(order);                
            }
            
            {
                var address = new Address("deep deep sea", "Park", "BL", 12345);
                var customer = new Customer(13, "Test3", address);
                var dishes = new List<Dishes> { new("WuDong"), new("XiaoLongBao")};
                var order = new SubmitData(customer, dishes, new PaymentInfo()) { Destination = address };
                yield return new SubmitOrder(order);                
            }

        }
    }


    protected override async Task<bool> DoOnce(long jobCounter)
    {
        if (jobCounter == 0)
        {
            foreach (var acceptOrder in PredefinedCommands)
            {
                await this.CommandBus.Execute(acceptOrder).ConfigureAwait(false);
            }

            return true;
        }

        return false;
    }
}
