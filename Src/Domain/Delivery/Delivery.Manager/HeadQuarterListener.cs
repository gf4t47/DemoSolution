// Copyright (c) Demo.
namespace Delivery;

using System;
using System.Threading.Tasks;
using Communication;
using Delivery.Command;
using Domain;
using Domain.Message;
using Microsoft.Extensions.Hosting;
public class HeadQuarterListener(IMessageQuerier<DeliveryScheduled> headQuarterReceiver, DeliveryCommandBus commandBus) : BackgroundLongRunner(TimeSpan.FromSeconds(1)), IHostedService
{
    private IMessageQuerier<DeliveryScheduled> HeadQuarterReceiver { get; } = headQuarterReceiver;
    
    private DeliveryCommandBus CommandBus { get; } = commandBus;

    protected override async Task<bool> DoOnce(long jobCounter)
    {
        var msg = await this.HeadQuarterReceiver.Receive().ConfigureAwait(false);
        if (msg is not null)
        {
            var payload = msg.Payload;
            Console.WriteLine($"{this.GetType().FullName} recv: {payload.Customer.FullName}@{payload.Customer.Id}, {payload.DeliveryAddress}");

            var data = new PlanDeliveryData(payload.Customer, payload.DeliveryAddress);
            var command = new PlanDelivery(data);
            await this.CommandBus.Execute(command).ConfigureAwait(false);
            
            return true;
        }

        return false;
    }
}
