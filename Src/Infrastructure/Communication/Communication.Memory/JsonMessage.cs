// Copyright (c) Demo.
namespace Communication;

using System.Collections.Generic;
public class JsonMessage<T>(T payload) : IMessage<T>
{
    public IDictionary<string, string> Headers { get; } = new Dictionary<string, string>();

    public T Payload { get; } = payload;
}
