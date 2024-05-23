﻿namespace Communication;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MessagePayload = System.Collections.Generic.IEnumerable<int>;
[TestClass]
public class MemoryQueueTests
{
    private static IEnumerable<IMessage<MessagePayload>> Messages
    {
        get
        {
            for (var i = 1; i < 5; i++)
            {
                var message = new Mock<IMessage<MessagePayload>>();
                message.SetupGet(m => m.Headers).Returns(new Dictionary<string, string> { ["type"] = nameof(MemoryQueueTests)});
                message.SetupGet(m => m.Payload).Returns(Enumerable.Range(1, i).ToList());

                yield return message.Object;                
            }
        }
    }

    private static IEnumerable<object[]> TestSendThenReceiveOneMessage
    {
        get
        {
            return Messages.Select(m => new object[] {m});
        }
    }
    
    [DataTestMethod]
    [DynamicData(nameof(TestSendThenReceiveOneMessage))]
    public async Task TestSendThenReceiveOneMessage_Succeed(IMessage<MessagePayload> msg)
    {
        var broker = new QueueMessageBroker();
        IMessageSender sender = new MemoryQueueSender(nameof(this.TestSendThenReceiveOneMessage_Succeed), broker);
        IMessageQuerier receiver = new MemoryQueueQuerier(nameof(this.TestSendThenReceiveOneMessage_Succeed), broker);

        var response = await sender.Send(msg).ConfigureAwait(false);
        Assert.AreEqual(ResponseType.Ack, response.Type);

        var actual = await receiver.Receive<MessagePayload>().ConfigureAwait(false);
        Assert.IsNotNull(actual);
        Assert.AreEqual(msg.Headers.Count, actual.Headers.Count);
        Assert.AreEqual(msg.Headers["type"], actual.Headers["type"]);
        Assert.AreEqual(msg.Payload.Count(), actual.Payload.Count());
        CollectionAssert.AreEqual(msg.Payload.ToList(), actual.Payload.ToList());
    }
    
    [DataTestMethod]
    [DynamicData(nameof(TestSendThenReceiveOneMessage))]
    public async Task TestSendOneMessageThenRetrieveTwice_Failed(IMessage<MessagePayload> msg)
    {
        var broker = new QueueMessageBroker();
        IMessageSender sender = new MemoryQueueSender(nameof(this.TestSendOneMessageThenRetrieveTwice_Failed), broker);
        IMessageQuerier receiver = new MemoryQueueQuerier(nameof(this.TestSendOneMessageThenRetrieveTwice_Failed), broker);

        var response = await sender.Send(msg).ConfigureAwait(false);
        Assert.AreEqual(ResponseType.Ack, response.Type);

        var first = await receiver.Receive<MessagePayload>().ConfigureAwait(false);
        Assert.IsNotNull(first);
        Assert.AreEqual(msg.Headers.Count, first.Headers.Count);
        Assert.AreEqual(msg.Headers["type"], first.Headers["type"]);
        Assert.AreEqual(msg.Payload.Count(), first.Payload.Count());
        CollectionAssert.AreEqual(msg.Payload.ToList(), first.Payload.ToList());
        
        var second = await receiver.Receive<MessagePayload>().ConfigureAwait(false);
        Assert.IsNull(second);
    }
    
    [DataTestMethod]
    [DynamicData(nameof(TestSendThenReceiveOneMessage))]
    public async Task TestSendRetrieveOnDiffTopic_Failed(IMessage<MessagePayload> msg)
    {
        var broker = new QueueMessageBroker();
        IMessageSender sender = new MemoryQueueSender(nameof(sender), broker);
        IMessageQuerier receiver = new MemoryQueueQuerier(nameof(receiver), broker);

        var response = await sender.Send(msg).ConfigureAwait(false);
        Assert.AreEqual(ResponseType.Ack, response.Type);

        var actual = await receiver.Receive<MessagePayload>().ConfigureAwait(false);
        Assert.IsNull(actual);
    }

    private static IEnumerable<object[]> TestSendThenReceiveMultipleMessage
    {
        get
        {
            var list = Messages.ToList();
            yield return [list];
        }
    }
    
    [DataTestMethod]
    [DynamicData(nameof(TestSendThenReceiveMultipleMessage))]
    public async Task TestSendThenReceiveMultipleMessage_Succeed(IList<IMessage<MessagePayload>> msgs)
    {
        var broker = new QueueMessageBroker();
        IMessageSender sender = new MemoryQueueSender(nameof(this.TestSendThenReceiveMultipleMessage_Succeed), broker);
        IMessageQuerier receiver = new MemoryQueueQuerier(nameof(this.TestSendThenReceiveMultipleMessage_Succeed), broker);

        foreach (var msg in msgs)
        {
            var response = await sender.Send(msg).ConfigureAwait(false);
            Assert.AreEqual(ResponseType.Ack, response.Type);
        }

        IList<IMessage<MessagePayload>> receivedList = new List<IMessage<MessagePayload>>();
        IMessage<MessagePayload>? received; 
        do
        {
            received = await receiver.Receive<MessagePayload>().ConfigureAwait(false);
            if (received is not null)
            {
                receivedList.Add(received);
            }
        } 
        while (received is not null);
        
        Assert.AreEqual(msgs.Count, receivedList.Count);
        for (var i = 0; i < receivedList.Count; i++)
        {
            var expected = msgs[i]; 
            var actual = receivedList[i];
            
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.Headers.Count, actual.Headers.Count);
            Assert.AreEqual(expected.Headers["type"], actual.Headers["type"]);
            Assert.AreEqual(expected.Payload.Count(), actual.Payload.Count());
            CollectionAssert.AreEqual(expected.Payload.ToList(), actual.Payload.ToList());            
        }
    }
}