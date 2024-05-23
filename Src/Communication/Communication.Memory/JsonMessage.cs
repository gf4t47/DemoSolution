// Copyright (c) Demo.
namespace Communication;

using System.Collections.Generic;
public class JsonMessage<T>(IDictionary<string, string> headers, T payload) : IMessage<T>
{
    public IDictionary<string, string> Headers { get; } = headers;

    public T Payload { get; } = payload;
}
