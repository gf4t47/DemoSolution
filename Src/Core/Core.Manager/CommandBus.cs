namespace Core;

using System;
using System.Threading.Tasks;
using Core.Command;
using Microsoft.Extensions.DependencyInjection;

public class CommandBus(IServiceProvider serviceProvider) : ITCommandBus
{
    private IServiceProvider ServiceProvider { get; } = serviceProvider;

    /// <summary>
    /// Dispatch command to registered command handler
    /// </summary>
    /// <param name="command">command to run.</param>
    /// <typeparam name="TInput">command handler route key.</typeparam>
    /// <returns></returns>
    public Task<bool> Execute<TInput>(IDemoCommand<TInput> command)
    {
        var handler = this.ServiceProvider.GetRequiredService<ICommandHandler<IDemoCommand<TInput>>>();
        return handler.Process(command);
    }
}
