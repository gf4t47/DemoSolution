// Copyright (c) Demo.
namespace Delivery.Handler;

using System;
using System.Threading.Tasks;
using Communication;
using Core.Command;
using Delivery.Command;
using Delivery.Message;
using Domain.Message;
using Domain.Model;
using Ordering.Model;
using Persistence;
public class MakeDeliveryHandler(IMessageSender<DeliveryCompleted> headQuarterSender, IRepository<Order> orderRepo) : ICommandHandler<MakeDelivery>
{
    private IMessageSender<DeliveryCompleted> HeadQuarterSender { get; } = headQuarterSender;
    
    private IRepository<Order> OrderRepo { get; } = orderRepo;

    public async Task<bool> Process(MakeDelivery command)
    {
        var data = command.Data;
        var customerAddress = await this.UpdateEntity(data.OrderId).ConfigureAwait(false);
        
        var payload = new DeliveryCompleted(data.OrderId, data.Customer, customerAddress);
        var msg = new DeliveryCompletedMessage(payload);
        var response = await this.HeadQuarterSender.Publish(msg).ConfigureAwait(false);

        Console.WriteLine($"{this.GetType().FullName} sent: {payload.Customer.FullName}@{data.OrderId} {data.ShopAddress} => {customerAddress}");        
        return response.Type == ResponseType.Ack;
    }

    private async Task<Address> UpdateEntity(int entityId)
    {
        var order = await this.OrderRepo.GetById(entityId).ConfigureAwait(false);
        order.Status = Order.StatusDeliveryCompleted;
        await this.OrderRepo.Update(order).ConfigureAwait(false);

        return order.DeliveryAddress;
    }
}
