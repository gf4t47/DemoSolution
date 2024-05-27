// Copyright (c) Demo.
namespace HeadQuarter;

using System;
using System.Threading;
using System.Threading.Tasks;
using Communication;
using Domain.Message;
using HeadQuarter.Command;
using HeadQuarter.Internal;
using Microsoft.Extensions.Hosting;
public class OrderingListener(IMessageQuerier<OrderApproved> orderingReceiver, HeadQuarterCommandBus commandBus) : IHostedService
{
    private IMessageQuerier<OrderApproved> OrderingReceiver { get; } = orderingReceiver;
    private HeadQuarterCommandBus CommandBus { get; } = commandBus;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(() => this.DoWork(cancellationToken), cancellationToken);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task DoWork(CancellationToken cancellationToken)
    {
        var count = 0;
        while (!cancellationToken.IsCancellationRequested)
        {
            var msg = await this.OrderingReceiver.Receive().ConfigureAwait(false);
            if (msg is not null)
            {
                var dishes = msg.Payload.ToDishes(msg.Headers);
                await this.CommandBus.Execute(new MakeDishes(dishes)).ConfigureAwait(false);

                var delivery = msg.Payload.ToDelivery(msg.Headers);
                await this.CommandBus.Execute(new DeliverDishes(delivery)).ConfigureAwait(false);
            }
            else
            {
                Console.WriteLine($"{nameof(OrderingListener)} is listening...{count}");
                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken).ConfigureAwait(false);
            }

            count++;
        }
    }
}
