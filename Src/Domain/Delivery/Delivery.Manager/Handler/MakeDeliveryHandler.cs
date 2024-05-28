// Copyright (c) Demo.
namespace Delivery.Handler;

using System;
using System.Threading.Tasks;
using Communication;
using Core.Command;
using Delivery.Command;
using Delivery.Message;
using Domain.Message;
public class MakeDeliveryHandler(IMessageSender<DeliveryCompleted> headQuarterSender) : ICommandHandler<MakeDelivery>
{
    private IMessageSender<DeliveryCompleted> HeadQuarterSender { get; } = headQuarterSender;

    public async Task<bool> Process(MakeDelivery command)
    {
        var data = command.Data;
        var payload = new DeliveryCompleted(data.Customer, data.DeliveryAddress);
        var msg = new DeliveryCompletedMessage(payload);
        var response = await this.HeadQuarterSender.Publish(msg).ConfigureAwait(false);

        Console.WriteLine($"{this.GetType().FullName} sent: {payload.Customer.FullName}@{payload.Customer.Id} => Todo, read address from Repo");        
        return response.Type == ResponseType.Ack;
    }
}
