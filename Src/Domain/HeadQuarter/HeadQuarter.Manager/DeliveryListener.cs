// Copyright (c) Demo.
namespace HeadQuarter;

using System;
using System.Threading.Tasks;
using Communication;
using Domain;
using Domain.Message;
using Microsoft.Extensions.Hosting;
public class DeliveryListener(IMessageQuerier<DeliveryCompleted> deliveryReceiver) : BackgroundLongRunner(TimeSpan.FromSeconds(1)), IHostedService
{
    private IMessageQuerier<DeliveryCompleted> DeliveryReceiver { get; } = deliveryReceiver;

    protected override async Task<bool> DoOnce(long jobCounter)
    {
        var msg = await this.DeliveryReceiver.Receive().ConfigureAwait(false);
        if (msg is not null)
        {
            var payload = msg.Payload;
            Console.WriteLine($"{this.GetType().FullName} recv: {payload.Customer.FullName}@{payload.OrderId}, {payload.DeliveryAddress}");
            return true;
        }

        return false;
    }
}
