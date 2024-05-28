1. Resolve `CommandBus` from DI.
2. Prepared a `Command` (from queue message or from other request).
3. `CommandBus.Execute(Command)`
4. Inside `Execute` method of `CommandBus`, Resolve `CommandHandler`  from DI.
5. `CommandHandler.Process(Command)`, by define different `handler` we can achieve different logic.
## Command

```C#
public interface IDemoCommand<out TData> : IDemoCommand  
{  
    public TData Data { get; }  
}
```

## Handler
```C#
public interface ICommandHandler<in TCommand> where TCommand : IDemoCommand  
{  
    Task<bool> Process(TCommand command);  
}
```

## Command Bus

Instead of introduce one more 3rd part library (e.g., MediatR) to scan the code (via reflection) to match command type to command handler.
We simplify to use `(dynamic)` to do a runtime dispatch.  
In real production code, we should consider `MediatR`

```C#
public class OrderingCommandBus(IServiceProvider serviceProvider) : ICommandBus  
{  
    private IServiceProvider ServiceProvider { get; } = serviceProvider;  
  
    public Task<bool> Execute(IDemoCommand command)  
    {
	    return this.Dispatch((dynamic)command);  
    }
      
    private Task<bool> Dispatch(AcceptOrder command)  
    {
	    var handler =
	    this.ServiceProvider.GetRequiredService<ICommandHandler<AcceptOrder>>();  
        return handler.Process(command);  
    }    
    
    private Task<bool> Dispatch(RejectOrder command)  
    {
	    var handler =
	    this.ServiceProvider.GetRequiredService<ICommandHandler<RejectOrder>>();  
        return handler.Process(command);  
    }    
    
    private Task<bool> Dispatch(CancelOrder command)  
    {
	    var handler =
	    this.ServiceProvider.GetRequiredService<ICommandHandler<CancelOrder>>();  
        return handler.Process(command);  
    }    
    
    private Task<bool> Dispatch(SubmitOrder command)  
    {
	    var handler = 
	    this.ServiceProvider.GetRequiredService<ICommandHandler<SubmitOrder>>();  
        return handler.Process(command);  
    }
}
```