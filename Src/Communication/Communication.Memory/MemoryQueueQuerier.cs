// Copyright (c) Demo.
namespace Communication.Memory;

using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Communication.Contracts;
public class MemoryQueueQuerier(string topic, QueueMessageBroker broker) : IMessageQuerier
{
    /// <summary>
    /// Topic to subscribe 
    /// </summary>
    private string Topic { get; } = topic;

    /// <summary>
    /// Wrap In-memory message queue
    /// </summary>
    private QueueMessageBroker Broker { get; } = broker;

    public Task<IMessage<T>?> Receive<T>()
    {
        if (this.Broker.TryDequeue(this.Topic, out var result))
        {
            var header = JsonSerializer.Deserialize<IDictionary<string, string>>(result.Item1)!;
            var payload = JsonSerializer.Deserialize<T>(result.Item2)!;

            return Task.FromResult<IMessage<T>?>(new JsonMessage<T>(header, payload));
        }

        return Task.FromResult<IMessage<T>?>(null);
    }
}
