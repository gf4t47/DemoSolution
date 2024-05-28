// Copyright (c) Demo.
namespace HeadQuarter.Handler;

using System;
using System.Threading.Tasks;
using Communication;
using Core.Command;
using Domain.Message;
using HeadQuarter.Command;
using HeadQuarter.Message;
public class MakeDishesHandler(IMessageSender<DishesScheduled> sender) : ICommandHandler<MakeDishes>
{
    private IMessageSender<DishesScheduled> Sender { get; } = sender;
    
    public async Task<bool> Process(MakeDishes command)
    {
        var data = command.Data;
        var payload = new DishesScheduled(data.Customer, data.Food);
        var msg = new DishesScheduledMessage(payload);
        var response = await this.Sender.Publish(msg).ConfigureAwait(false);
        
        Console.WriteLine($"{this.GetType().FullName} sent: {payload.Customer.FullName}@{payload.Customer.Id}, [{string.Join(",", payload.Food)}]");
        return response.Type == ResponseType.Ack;
    }
}
