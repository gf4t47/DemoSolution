namespace Communication.Memory;

using System.Collections.Concurrent;

using TQueueItem = (string, string);

public class QueueMessageBroker
{
    private ConcurrentDictionary<string, ConcurrentQueue<TQueueItem>> QueueBus { get;} = new ();


    public void Enqueue(string topic, TQueueItem body)
    {
        var queue = this.QueueBus.GetOrAdd(topic, new ConcurrentQueue<TQueueItem>());
        queue.Enqueue(body);
    }

    public bool TryDequeue(string topic, out TQueueItem result)
    {
        if (this.QueueBus.TryGetValue(topic, out var queue))
        {
            return queue.TryDequeue(out result);
        }

        result = (string.Empty, string.Empty);
        return false;
    }
}
