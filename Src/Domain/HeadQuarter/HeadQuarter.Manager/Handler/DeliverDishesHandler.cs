// Copyright (c) Demo.
namespace HeadQuarter.Handler;

using System;
using System.Threading.Tasks;
using Communication;
using Core.Command;
using Domain.Message;
using HeadQuarter.Command;
using HeadQuarter.Message;
public class DeliverDishesHandler(IMessageSender<DeliveryScheduled> sender) : ICommandHandler<DeliverDishes>
{
    private IMessageSender<DeliveryScheduled> Sender { get; } = sender;

    public async Task<bool> Process(DeliverDishes command)
    {
        var data = command.Data;
        var payload = new DeliveryScheduled(data.Customer, data.DeliveryAddress);
        var msg = new DeliveryScheduledMessage(payload);
        var response = await this.Sender.Publish(msg).ConfigureAwait(false);
        
        Console.WriteLine($"{this.GetType().Name} sent: {payload.Customer.FullName}@{payload.Customer.Id}, {payload.DeliveryAddress}");
        return response.Type == ResponseType.Ack;
    }
}
