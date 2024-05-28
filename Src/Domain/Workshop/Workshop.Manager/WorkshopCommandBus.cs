// Copyright (c) Demo.
namespace Workshop;

using System;
using System.Threading.Tasks;
using Core;
using Core.Command;
using Microsoft.Extensions.DependencyInjection;
using Workshop.Command;
public class WorkshopCommandBus(IServiceProvider serviceProvider) : ICommandBus
{
    private IServiceProvider ServiceProvider { get; } = serviceProvider;

    public Task<bool> Execute(IDemoCommand command)
    {
        return this.Dispatch((dynamic)command);
    }

    private Task<bool> Dispatch(Cooking command)
    {
        var handler = this.ServiceProvider.GetRequiredService<ICommandHandler<Cooking>>();
        return handler.Process(command);
    }
}
