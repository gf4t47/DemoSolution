// Copyright (c) Demo.
namespace HeadQuarter;

using System;
using System.Threading.Tasks;
using Communication;using Domain;
using Domain.Message;
using HeadQuarter.Command;
using HeadQuarter.Internal;
using Microsoft.Extensions.Hosting;
public class OrderingListener(IMessageQuerier<OrderApproved> orderingReceiver, HeadQuarterCommandBus commandBus) : BackgroundLongRunner(TimeSpan.FromSeconds(1)), IHostedService
{
    private IMessageQuerier<OrderApproved> OrderingReceiver { get; } = orderingReceiver;
    private HeadQuarterCommandBus CommandBus { get; } = commandBus;

    protected override async Task<bool> DoOnce(int jobCounter)
    {
        var msg = await this.OrderingReceiver.Receive().ConfigureAwait(false);
        if (msg is not null)
        {
            var payload = msg.Payload;
            Console.WriteLine($"{this.GetType().Name} recv: customer{payload.Customer.Id}, [{string.Join(",", payload.Food)}], {payload.DeliveryAddress}");
                
            var dishes = payload.ToDishes(msg.Headers);
            await this.CommandBus.Execute(new MakeDishes(dishes)).ConfigureAwait(false);

            var delivery = payload.ToDelivery(msg.Headers);
            await this.CommandBus.Execute(new DeliverDishes(delivery)).ConfigureAwait(false);
            return true;
        }

        return false;
    }
}
