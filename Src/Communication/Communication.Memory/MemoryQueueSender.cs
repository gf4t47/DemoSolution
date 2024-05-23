// Copyright (c) Demo.
namespace Communication.Memory;

using System.Text.Json;
using System.Threading.Tasks;
using Communication.Contracts;
public class MemoryQueueSender(string topic, QueueMessageBroker broker) : IMessageSender
{
    /// <summary>
    /// Topic to subscribe 
    /// </summary>
    private string Topic { get; } = topic;

    /// <summary>
    /// Wrap In-memory message queue
    /// </summary>
    private QueueMessageBroker Broker { get; } = broker;

    public Task<PublishResponse> Send<T>(IMessage<T> message)
    {
        var headers = JsonSerializer.Serialize(message.Headers);
        var payload = JsonSerializer.Serialize(message.Payload);
        
        this.Broker.Enqueue(this.Topic, (headers, payload));
        return Task.FromResult(new PublishResponse(ResponseType.Ack));
    }
}
