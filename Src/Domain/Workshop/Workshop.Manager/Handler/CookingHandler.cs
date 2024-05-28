// Copyright (c) Demo.
namespace Workshop.Handler;

using System;
using System.Threading.Tasks;
using Communication;
using Core.Command;
using Domain.Message;
using Domain.Model;
using Ordering.Model;
using Persistence;
using Workshop.Command;
using Workshop.Message;
public class CookingHandler(IMessageSender<DishesReady> deliverySender, IRepository<Order> orderRepo) : ICommandHandler<Cooking>
{
    private static readonly Address ShopAddress = new Address("WorkShop", "ShopCity", "ShopState", 98765);
    
    private IMessageSender<DishesReady> DeliverySender { get; } = deliverySender;
    
    private IRepository<Order> OrderRepo { get; } = orderRepo;

    public async Task<bool> Process(Cooking command)
    {
        var data = command.Data;
        var payload = new DishesReady(data.OrderId, data.Customer, data.ToCook, ShopAddress);
        var msg = new DishReadyMessage(payload);
        var response = await this.DeliverySender.Publish(msg).ConfigureAwait(false);

        await this.UpdateEntity(data.OrderId).ConfigureAwait(false);
        Console.WriteLine($"{this.GetType().FullName} sent: {payload.Customer.FullName}@{data.OrderId}, [{string.Join(",", payload.Food)}]");
        return response.Type == ResponseType.Ack;
    }

    private async Task<bool> UpdateEntity(int entityId)
    {
        var order = await this.OrderRepo.GetById(entityId).ConfigureAwait(false);
        order.Status = Order.StatusDishesReady;
        return await this.OrderRepo.Update(order).ConfigureAwait(false);
    }
}
