// Copyright (c) Demo.
namespace Ordering;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;using Domain;
using Domain.Model;
using Microsoft.Extensions.Hosting;
using Ordering.Command;
public class OrderingHostService(OrderingCommandBus commandBus) : BackgroundLongRunner(TimeSpan.FromSeconds(10)), IHostedService
{
    private OrderingCommandBus CommandBus { get; } = commandBus;

    /// <summary>
    /// Mock orders for console demo purpose
    /// </summary>
    private static IEnumerable<AcceptOrder> PredefinedCommands
    {
        get
        {
            var address = new Address("street", "LA", "CA", 91100);
            var customer = new Customer(1, "Test", address);
            var dishes = new List<Dishes> { new("Pizza"), new("Noodles")};
            var order = new VerifyData(customer, dishes, address);
            yield return new AcceptOrder(order);
        }
    }


    protected override async Task<bool> DoOnce(int jobCounter)
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
