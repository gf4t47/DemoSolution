// Copyright (c) Demo.
namespace Delivery;

using Communication;
using Core.Command;
using Delivery.Command;
using Delivery.Handler;
using Domain;
using Domain.Message;
using Microsoft.Extensions.DependencyInjection;
public static class Configuration
{
    public static IServiceCollection ConfigDeliveryCommandHandler(this IServiceCollection sc)
    {
        sc.AddTransient<ICommandHandler<PlanDelivery>, PlanDeliveryHandler>();
        sc.AddTransient<ICommandHandler<MakeDelivery>, MakeDeliveryHandler>();
        sc.AddSingleton<DeliveryCommandBus>();

        return sc;
    }
    
    public static IServiceCollection ConfigDeliveryCommunication(this IServiceCollection sc)
    {
        sc.AddSingleton<QueueMessageBroker>();

        {
            var (name, channel) = typeof(DeliveryScheduled).ResolveMessageChannel();
            sc.Configure<MessageChannel>(name, opt => opt.Topic = channel);
            sc.AddTransient<IMessageQuerier<DeliveryScheduled>, MemoryQueueQuerier<DeliveryScheduled>>();            
        }

        {
            var (name, channel) = typeof(DishesReady).ResolveMessageChannel();
            sc.Configure<MessageChannel>(name, opt => opt.Topic = channel);
            sc.AddTransient<IMessageQuerier<DishesReady>, MemoryQueueQuerier<DishesReady>>();
        }

        {
            var (name, channel) = typeof(DeliveryCompleted).ResolveMessageChannel();
            sc.Configure<MessageChannel>(name, opt => opt.Topic = channel);
            sc.AddTransient<IMessageSender<DeliveryCompleted>, MemoryQueueSender<DeliveryCompleted>>();
        }
        
        return sc;
    }
    
    public static IServiceCollection ConfigDeliveryHostService(this IServiceCollection sc)
    {
        sc.AddHostedService<HeadQuarterListener>();
        sc.AddHostedService<WorkshopListener>();
        return sc;
    }
}
