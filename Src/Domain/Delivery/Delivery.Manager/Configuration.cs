// Copyright (c) Demo.
namespace Delivery;

using Communication;
using Domain.Message;
using Microsoft.Extensions.DependencyInjection;
public static class Configuration
{
    public static IServiceCollection ConfigDeliveryCommunication(this IServiceCollection sc)
    {
        sc.AddSingleton<QueueMessageBroker>();
        sc.AddTransient<IMessageQuerier<DeliveryScheduled>, MemoryQueueQuerier<DeliveryScheduled>>();
        
        return sc;
    }
    
    public static IServiceCollection ConfigDeliveryHostService(this IServiceCollection sc)
    {
        sc.AddHostedService<HeadQuarterListener>();
        return sc;
    }
}
