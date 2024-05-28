// Copyright (c) Demo.
namespace Delivery;

using System;
using System.Threading.Tasks;
using Communication;
using Domain;
using Domain.Message;
using Microsoft.Extensions.Hosting;
public class HeadQuarterListener(IMessageQuerier<DeliveryScheduled> deliveryReceiver) : BackgroundLongRunner(TimeSpan.FromSeconds(1)), IHostedService
{
    private IMessageQuerier<DeliveryScheduled> DeliveryReceiver { get; } = deliveryReceiver;

    protected override async Task<bool> DoOnce(long jobCounter)
    {
        var msg = await this.DeliveryReceiver.Receive().ConfigureAwait(false);
        if (msg is not null)
        {
            var payload = msg.Payload;
            Console.WriteLine($"{this.GetType().FullName} recv: {payload.Customer.FullName}@{payload.Customer.Id}, {payload.DeliveryAddress}");
            
            return true;
        }

        return false;
    }
}
