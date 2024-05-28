// Copyright (c) Demo.
namespace Delivery.Handler;

using System;
using System.Threading.Tasks;
using Core.Command;
using Delivery.Command;
public class PlanDeliveryHandler : ICommandHandler<PlanDelivery>
{
    public Task<bool> Process(PlanDelivery command)
    {
        // make plan => do nothing here in code.
        Console.WriteLine($"{this.GetType().FullName} plan: {command.Data.Customer.FullName}@{command.Data.OrderId} => {command.Data.DeliveryAddress}");
        return Task.FromResult(true);
    }
}
