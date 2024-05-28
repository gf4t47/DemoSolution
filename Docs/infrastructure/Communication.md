- Add a abstract async communication layer first,
- First Implement an in-memory `ConcurrentQueue` to achieve a MVP asap.
- Would consider `RabbitMQ` if have more time work on it. 

### Message Publisher
```C#
public interface IMessageQuerier<T>  
{  
    Task<IMessage<T>?> Receive();  
}
```

### Message Receiver
```C#
public interface IMessageSender<in T>  
{  
    Task<PublishResponse> Publish(IMessage<T> message);  
}
```

### Message Subscriber
- An option for continuous streaming of data.
- Make `IHostedService` to continue running in backend to poll message from queue.

```C#
public abstract class BackgroundLongRunner(TimeSpan waitTimeWhenNoJob)  
{  
    private TimeSpan WaitTimeWhenNoJob { get; } = waitTimeWhenNoJob;  
    public Task StartAsync(CancellationToken cancellationToken)  
    {        
	    Task.Run(() => this.DoWork(cancellationToken), cancellationToken);  
        // _ = this.DoWork(cancellationToken);  
        return Task.CompletedTask;  
    }
      
    public Task StopAsync(CancellationToken cancellationToken)  
    {
	    return Task.CompletedTask;  
    }
      
    private async Task DoWork(CancellationToken cancellationToken)  
    {
	    long count = 0;  
        while (!cancellationToken.IsCancellationRequested)  
        {            
	        var doneOnce = await this.DoOnce(count).ConfigureAwait(false);  
            if (!doneOnce)  
            {
	            await Task.Delay(this.WaitTimeWhenNoJob, cancellationToken)
	            .ConfigureAwait(false);                  
            }  
            count = (count + 1) % long.MaxValue;  
        }    
    }  
    
    protected abstract Task<bool> DoOnce(long jobCounter);  
}
```

`RunOnce` implementer
```C#
public class WorkshopListener(IMessageQuerier<DishesReady> workshopReceiver, DeliveryCommandBus commandBus) : BackgroundLongRunner(TimeSpan.FromSeconds(1)), IHostedService  
{  
    private IMessageQuerier<DishesReady> WorkshopReceiver { get; } = workshopReceiver;  
    private DeliveryCommandBus CommandBus { get; } = commandBus;  
  
    protected override async Task<bool> DoOnce(long jobCounter)  
    {
        var msg = await this.WorkshopReceiver.Receive().ConfigureAwait(false);  
        if (msg is not null)  
        {
            var payload = msg.Payload;  
            Console.WriteLine($"{this.GetType().FullName} recv: {payload.Customer.FullName}@{payload.OrderId}, [{string.Join(",", payload.Food)}]");  
  
            var data = new MakeDeliveryData(payload.OrderId, payload.Customer, payload.ShopAddress);  
            var command = new MakeDelivery(data);  
            await this.CommandBus.Execute(command).ConfigureAwait(false);  
  
            return true;  
        }  
        return false;  
    }}
```
### Message Type to Channel Map
All message type (payload type) are stored in one single project, so we can maintains `message type` => `message channel(topic)` map easily.
```C#
private static Dictionary<string, string> MessageType2MessageChannel { get; } = new()  
{  
    [nameof(OrderApproved)] = "Ordering=>HeadQuarters",  
    [nameof(DishesScheduled)] = "HeadQuarters=>Workshop",  
    [nameof(DeliveryScheduled)] = "HeadQuarters=>Delivery"  
};  
  
public static (string, string) ResolveMessageChannel(this Type payloadType)  
{  
    var key = payloadType.Name;  
    if (MessageType2MessageChannel.TryGetValue(key, out var channel))  
    {        return (key, channel);  
    }  
    throw new ArgumentException($"Unknown payload type: {key}");  
}
```
