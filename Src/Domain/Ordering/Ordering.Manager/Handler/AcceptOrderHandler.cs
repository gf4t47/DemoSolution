﻿// Copyright (c) Demo.
namespace Ordering.Handler;

using System;
using System.Threading.Tasks;
using Communication;
using Core.Command;
using Domain.Message;
using Ordering.Command;
using Ordering.Message;
public class AcceptOrderHandler(IMessageSender<OrderApproved> sender) : ICommandHandler<AcceptOrder>
{
    private IMessageSender<OrderApproved> Sender { get; } = sender;

    public async Task<bool> Process(AcceptOrder command)
    {
        var data = command.Data;
        var payload = new OrderApproved(data.OrderId, data.Customer, data.Food, data.DeliveryAddress);
        var msg = new OrderApprovedMessage(payload);
        var response = await this.Sender.Publish(msg).ConfigureAwait(false);
        
        Console.WriteLine($"{this.GetType().FullName} sent: {payload.Customer.FullName}@{payload.OrderId}, [{string.Join(",", payload.Food)}], {payload.DeliveryAddress}");
        return response.Type == ResponseType.Ack;
    }
}
