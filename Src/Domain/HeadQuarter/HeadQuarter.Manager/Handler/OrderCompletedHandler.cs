// Copyright (c) Demo.
namespace HeadQuarter.Handler;

using System;
using System.Threading.Tasks;
using Core.Command;
using HeadQuarter.Command;
using Ordering.Model;
using Persistence;
public class OrderCompletedHandler(IRepository<Order> orderRepo) : ICommandHandler<OrderCompleted>
{
    private IRepository<Order> OrderRepo { get; } = orderRepo;

    public Task<bool> Process(OrderCompleted command)
    {
        var data = command.Data;
        Console.WriteLine($"{this.GetType().FullName}: Complete Order {data.OrderId} for {data.Customer.FullName} ");
        return Task.FromResult(true);
    }

    private async Task<bool> UpdateEntity(int entityId)
    {
        var order = await this.OrderRepo.GetById(entityId).ConfigureAwait(false);
        order.Status = Order.StatusCompleted;
        return await this.OrderRepo.Update(order).ConfigureAwait(false);
    }
}
