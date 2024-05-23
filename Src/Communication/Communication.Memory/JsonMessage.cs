// Copyright (c) Demo.
namespace Communication.Memory;

using System.Collections.Generic;
using Communication.Contracts;
public class JsonMessage<T>(IDictionary<string, string> headers, T payload) : IMessage<T>
{
    public IDictionary<string, string> Headers { get; } = headers;

    public T Payload { get; } = payload;
}
