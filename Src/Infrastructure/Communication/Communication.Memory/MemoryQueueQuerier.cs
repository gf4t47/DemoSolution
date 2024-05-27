// Copyright (c) Demo.
namespace Communication;

using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
public class MemoryQueueQuerier<T>(IOptions<MessageChannel> option, QueueMessageBroker broker) : IMessageQuerier<T>
{
    /// <summary>
    /// Topic to subscribe 
    /// </summary>
    private string Topic => this.Channel.Topic;

    /// <summary>
    /// Channel option from DI
    /// </summary>
    private MessageChannel Channel { get; } = option.Value;
    
    /// <summary>
    /// Wrap In-memory message queue
    /// </summary>
    private QueueMessageBroker Broker { get; } = broker;

    public Task<IMessage<T>?> Receive()
    {
        if (this.Broker.TryDequeue(this.Topic, out var result))
        {
            var header = JsonSerializer.Deserialize<IDictionary<string, string>>(result.Item1) ?? new Dictionary<string, string>();
            var payload = JsonSerializer.Deserialize<T>(result.Item2)!;
            var msg = new JsonMessage<T>(payload);
            foreach (var kvp in header)
            {
                msg.Headers[kvp.Key] = kvp.Value;
            }

            return Task.FromResult<IMessage<T>?>(msg);
        }

        return Task.FromResult<IMessage<T>?>(null);
    }
}
