// Copyright (c) Demo.
namespace HeadQuarter;

using System;
using System.Threading.Tasks;
using Core;
using Core.Command;
using HeadQuarter.Command;
using Microsoft.Extensions.DependencyInjection;
public class HeadQuarterCommandBus(IServiceProvider serviceProvider) : ICommandBus
{
    private IServiceProvider ServiceProvider { get; } = serviceProvider;

    public Task<bool> Execute(IDemoCommand command)
    {
        return this.Dispatch((dynamic)command);
    }

    private Task<bool> Dispatch(MakeDishes command)
    {
        var handler = this.ServiceProvider.GetRequiredService<ICommandHandler<MakeDishes>>();
        return handler.Process(command);
    }

    private Task<bool> Dispatch(DeliverDishes command)
    {
        var handler = this.ServiceProvider.GetRequiredService<ICommandHandler<DeliverDishes>>();
        return handler.Process(command);
    }
}
