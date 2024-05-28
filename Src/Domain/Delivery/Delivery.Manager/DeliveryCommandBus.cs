// Copyright (c) Demo.
namespace Delivery;

using System;
using System.Threading.Tasks;
using Core;
using Core.Command;
using Delivery.Command;
using Microsoft.Extensions.DependencyInjection;
public class DeliveryCommandBus(IServiceProvider serviceProvider) : ICommandBus
{
    private IServiceProvider ServiceProvider { get; } = serviceProvider;

    public Task<bool> Execute(IDemoCommand command)
    {
        return this.Dispatch((dynamic)command);
    }

    private Task<bool> Dispatch(PlanDelivery command)
    {
        var handler = this.ServiceProvider.GetRequiredService<ICommandHandler<PlanDelivery>>();
        return handler.Process(command);
    }

    private Task<bool> Dispatch(MakeDelivery command)
    {
        var handler = this.ServiceProvider.GetRequiredService<ICommandHandler<MakeDelivery>>();
        return handler.Process(command);
    }
}
