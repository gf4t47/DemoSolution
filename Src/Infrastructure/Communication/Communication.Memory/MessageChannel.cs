// Copyright (c) Demo.
namespace Communication;

public record MessageChannel(string Topic)
{
    private const string DefaultTopic = "default";

    public MessageChannel() : this(DefaultTopic)
    {
        
    }

    public string Topic { get; set; } = Topic;
}
