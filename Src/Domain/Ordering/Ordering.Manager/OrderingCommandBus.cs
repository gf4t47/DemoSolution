// Copyright (c) Demo.
namespace Ordering;

using System;
using System.Threading.Tasks;
using Core;
using Core.Command;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Command;
public class OrderingCommandBus(IServiceProvider serviceProvider) : ICommandBus
{
    private IServiceProvider ServiceProvider { get; } = serviceProvider;

    /// <summary>
    /// Instead of introduce one more 3rd part library (e.g., MediatR) to scan the code (reflection) to match command type to command handler,
    /// We simplify to use (dynamic) to do a runtime dispatch.
    /// In real production code, we should consider `MediatR`
    /// </summary>
    /// <param name="command">command to exec.</param>
    /// <returns></returns>
    public Task<bool> Execute(IDemoCommand command)
    {
        return this.Dispatch((dynamic)command);
    }

    private Task<bool> Dispatch(AcceptOrder command)
    {
        var handler = this.ServiceProvider.GetRequiredService<ICommandHandler<AcceptOrder>>();
        return handler.Process(command);
    }
    
    private Task<bool> Dispatch(RejectOrder command)
    {
        var handler = this.ServiceProvider.GetRequiredService<ICommandHandler<RejectOrder>>();
        return handler.Process(command);
    }
    
    private Task<bool> Dispatch(CancelOrder command)
    {
        var handler = this.ServiceProvider.GetRequiredService<ICommandHandler<CancelOrder>>();
        return handler.Process(command);
    }
    
    private Task<bool> Dispatch(SubmitOrder command)
    {
        var handler = this.ServiceProvider.GetRequiredService<ICommandHandler<SubmitOrder>>();
        return handler.Process(command);
    }
}
