// Copyright (c) Demo.
namespace Delivery;

using System;
using System.Threading.Tasks;
using Communication;
using Delivery.Command;
using Domain;
using Domain.Message;
using Microsoft.Extensions.Hosting;
public class WorkshopListener(IMessageQuerier<DishesReady> workshopReceiver, DeliveryCommandBus commandBus) : BackgroundLongRunner(TimeSpan.FromSeconds(1)), IHostedService
{
    private IMessageQuerier<DishesReady> WorkshopReceiver { get; } = workshopReceiver;
    
    private DeliveryCommandBus CommandBus { get; } = commandBus;

    protected override async Task<bool> DoOnce(long jobCounter)
    {
        var msg = await this.WorkshopReceiver.Receive().ConfigureAwait(false);
        if (msg is not null)
        {
            var payload = msg.Payload;
            Console.WriteLine($"{this.GetType().FullName} recv: {payload.Customer.FullName}@{payload.Customer.Id}, [{string.Join(",", payload.Food)}]");

            var data = new MakeDeliveryData(payload.Customer);
            var command = new MakeDelivery(data);
            await this.CommandBus.Execute(command).ConfigureAwait(false);

            return true;
        }

        return false;
    }
}
