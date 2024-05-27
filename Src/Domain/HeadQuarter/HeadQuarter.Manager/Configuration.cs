namespace HeadQuarter;

using Communication;
using Core.Command;
using Domain;
using Domain.Message;
using HeadQuarter.Command;
using HeadQuarter.Handler;
using Microsoft.Extensions.DependencyInjection;

public static class Configuration
{
    public static IServiceCollection ConfigHeadQuarterCommandHandler(this IServiceCollection sc)
    {
        sc.AddTransient<ICommandHandler<MakeDishes>, MakeDishesHandler>();
        sc.AddTransient<ICommandHandler<DeliverDishes>, DeliverDishesHandler>();

        sc.AddSingleton<HeadQuarterCommandBus>();

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
 

        return sc;
    }
}
