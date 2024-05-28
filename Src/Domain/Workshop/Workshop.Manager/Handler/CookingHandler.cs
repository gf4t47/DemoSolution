// Copyright (c) Demo.
namespace Workshop.Handler;

using System;
using System.Threading.Tasks;
using Communication;
using Core.Command;
using Domain.Message;
using Workshop.Command;
using Workshop.Message;
public class CookingHandler(IMessageSender<DishesReady> deliverySender) : ICommandHandler<Cooking>
{
    private IMessageSender<DishesReady> DeliverySender { get; } = deliverySender;

    public async Task<bool> Process(Cooking command)
    {
        var data = command.Data;
        var payload = new DishesReady(data.Customer, data.ToCook);
        var msg = new DishReadyMessage(payload);
        var response = await this.DeliverySender.Publish(msg).ConfigureAwait(false);

        Console.WriteLine($"{this.GetType().FullName} sent: {payload.Customer.FullName}@{payload.Customer.Id}, [{string.Join(",", payload.Food)}]");
        return response.Type == ResponseType.Ack;
    }
}
