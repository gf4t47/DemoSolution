// Copyright (c) Demo.
namespace Domain;

using System;
using System.Collections.Generic;
using Domain.Message;
public static class MessageChannelMap
{
    private static Dictionary<string, string> MessageType2MessageChannel { get; } = new()
    {
        [nameof(OrderApproved)] = "Ordering=>HeadQuarters",
        [nameof(DishesScheduled)] = "HeadQuarters=>Workshop",
        [nameof(DeliveryScheduled)] = "HeadQuarters=>Delivery",
        [nameof(DishesReady)] = "Workshop=>Delivery",
        [nameof(DeliveryCompleted)] = "Delivery=>HeadQuarters"
    };

    public static (string, string) ResolveMessageChannel(this Type payloadType)
    {
        var key = payloadType.Name;
        if (MessageType2MessageChannel.TryGetValue(key, out var channel))
        {
            return (key, channel);
        }

        throw new ArgumentException($"Unknown payload type: {key}");
    } 
}
