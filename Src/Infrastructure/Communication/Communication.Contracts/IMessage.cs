// Copyright (c) Demo.
namespace Communication;

using System.Collections.Generic;
public interface IMessage<out TPayload>
{
    IDictionary<string, string> Headers { get; }
    
    TPayload Payload { get; }
}
