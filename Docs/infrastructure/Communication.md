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

### Message Subscriber
```C#
public interface IMessageSender<in T>  
{  
    Task<PublishResponse> Publish(IMessage<T> message);  
}
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

## TODO

1. An option for continuous streaming of data.