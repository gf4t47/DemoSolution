// Copyright (c) Demo.
namespace HeadQuarter.Handler;

using System;
using System.Threading.Tasks;
using Communication;
using Core.Command;
using Domain.Message;
using HeadQuarter.Command;
using HeadQuarter.Message;
using Ordering.Model;
using Persistence;
public class MakeDishesHandler(IMessageSender<DishesScheduled> sender, IRepository<Order> orderRepo) : ICommandHandler<MakeDishes>
{
    private IMessageSender<DishesScheduled> Sender { get; } = sender;
    
    private IRepository<Order> OrderRepo { get; } = orderRepo;

    public async Task<bool> Process(MakeDishes command)
    {
        var data = command.Data;
        var payload = new DishesScheduled(data.OrderId, data.Customer, data.Food);
        var msg = new DishesScheduledMessage(payload);
        var response = await this.Sender.Publish(msg).ConfigureAwait(false);

        await this.UpdateEntity(data.OrderId).ConfigureAwait(false);
        
        Console.WriteLine($"{this.GetType().FullName} sent: {payload.Customer.FullName}@{payload.OrderId}, [{string.Join(",", payload.Food)}]");
        return response.Type == ResponseType.Ack;
    }

    private async Task<bool> UpdateEntity(int entityId)
    {
        var order = await this.OrderRepo.GetById(entityId).ConfigureAwait(false);
        order.Status = Order.StatusDishesScheduled;
        return await this.OrderRepo.Update(order).ConfigureAwait(false);        
    }
}
