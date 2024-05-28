namespace HeadQuarter;

using Communication;
using Core.Command;
using Domain;
using Domain.Message;
using HeadQuarter.Command;
using HeadQuarter.Handler;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Model;
using Persistence;
public static class Configuration
{
    public static IServiceCollection ConfigHeadQuarterCommandHandler(this IServiceCollection sc)
    {
        sc.AddTransient<ICommandHandler<MakeDishes>, MakeDishesHandler>();
        sc.AddTransient<ICommandHandler<DeliverDishes>, DeliverDishesHandler>();

        sc.AddSingleton<HeadQuarterCommandBus>();

        return sc;
    }

    public static IServiceCollection ConfigHeadQuarterPersistence(this IServiceCollection sc)
    {
        sc.AddSingleton<IRepository<Order>, MemoryRepository<Order>>();

        return sc;
    }
    
    public static IServiceCollection ConfigHeadQuarterCommunication(this IServiceCollection sc)
    {
        sc.AddSingleton<QueueMessageBroker>();

        {
            var (name, channel) = typeof(OrderApproved).ResolveMessageChannel();
            sc.Configure<MessageChannel>(name, opt => opt.Topic = channel);
            sc.AddTransient<IMessageQuerier<OrderApproved>, MemoryQueueQuerier<OrderApproved>>();            
        }

        {
            var (name, channel) = typeof(DishesScheduled).ResolveMessageChannel();
            sc.Configure<MessageChannel>(name, opt => opt.Topic = channel);
            sc.AddTransient<IMessageSender<DishesScheduled>, MemoryQueueSender<DishesScheduled>>();
        }

        {
            var (name, channel) = typeof(DeliveryScheduled).ResolveMessageChannel();
            sc.Configure<MessageChannel>(name, opt => opt.Topic = channel);
            sc.AddTransient<IMessageSender<DeliveryScheduled>, MemoryQueueSender<DeliveryScheduled>>();
        }

        {
            var (name, channel) = typeof(DeliveryCompleted).ResolveMessageChannel();
            sc.Configure<MessageChannel>(name, opt => opt.Topic = channel);
            sc.AddTransient<IMessageQuerier<DeliveryCompleted>, MemoryQueueQuerier<DeliveryCompleted>>();
        }
 

        return sc;
    }

    public static IServiceCollection ConfigHeadQuarterHostService(this IServiceCollection sc)
    {
        sc.AddHostedService<OrderingListener>();
        sc.AddHostedService<DeliveryListener>();
        return sc;
    }
}
