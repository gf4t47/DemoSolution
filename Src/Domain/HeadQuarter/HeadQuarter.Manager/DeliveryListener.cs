// Copyright (c) Demo.
namespace HeadQuarter;

using System;
using System.Threading.Tasks;
using Communication;
using Domain;
using Domain.Message;
using HeadQuarter.Command;
using Microsoft.Extensions.Hosting;
public class DeliveryListener(IMessageQuerier<DeliveryCompleted> deliveryReceiver, HeadQuarterCommandBus commandBus) : BackgroundLongRunner(TimeSpan.FromSeconds(1)), IHostedService
{
    private IMessageQuerier<DeliveryCompleted> DeliveryReceiver { get; } = deliveryReceiver;
    
    private HeadQuarterCommandBus CommandBus { get; } = commandBus;

    protected override async Task<bool> DoOnce(long jobCounter)
    {
        var msg = await this.DeliveryReceiver.Receive().ConfigureAwait(false);
        if (msg is not null)
        {
            var payload = msg.Payload;
            Console.WriteLine($"{this.GetType().FullName} recv: {payload.Customer.FullName}@{payload.OrderId}, {payload.DeliveryAddress}");

            var data = new OrderCompletedData(payload.OrderId, payload.Customer);
            var command = new OrderCompleted(data);
            await this.CommandBus.Execute(command).ConfigureAwait(false);
            
            return true;
        }

        return false;
    }
}
