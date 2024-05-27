// Copyright (c) Demo.
namespace Communication;

using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
public class MemoryQueueSender<T>(IOptionsSnapshot<MessageChannel> option, QueueMessageBroker broker) : IMessageSender<T>
{
    /// <summary>
    /// Topic to subscribe 
    /// </summary>
    private string Topic => this.Option.Topic;

    /// <summary>
    /// Options get from DI
    /// </summary>
    private MessageChannel Option { get; } = option.Get(typeof(T).Name);
    
    /// <summary>
    /// Wrap In-memory message queue
    /// </summary>
    private QueueMessageBroker Broker { get; } = broker;

    public Task<PublishResponse> Publish(IMessage<T> message)
    {
        var headers = JsonSerializer.Serialize(message.Headers);
        var payload = JsonSerializer.Serialize(message.Payload);
        
        this.Broker.Enqueue(this.Topic, (headers, payload));
        return Task.FromResult(new PublishResponse(ResponseType.Ack));
    }
}
