// Copyright (c) Demo.
namespace Workshop;

using Communication;
using Core.Command;
using Domain;
using Domain.Message;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Model;
using Persistence;
using Workshop.Command;
using Workshop.Handler;
public static class Configuration
{
    public static IServiceCollection ConfigWorkshopCommandHandler(this IServiceCollection sc)
    {
        sc.AddTransient<ICommandHandler<Cooking>, CookingHandler>();
        sc.AddSingleton<WorkshopCommandBus>();

        return sc;
    }
    
    public static IServiceCollection ConfigWorkshopCommunication(this IServiceCollection sc)
    {
        sc.AddSingleton<QueueMessageBroker>();

        {
            var (name, channel) = typeof(DishesScheduled).ResolveMessageChannel();
            sc.Configure<MessageChannel>(name, opt => opt.Topic = channel);
            sc.AddTransient<IMessageQuerier<DishesScheduled>, MemoryQueueQuerier<DishesScheduled>>();            
        }

        {
            var (name, channel) = typeof(DishesReady).ResolveMessageChannel();
            sc.Configure<MessageChannel>(name, opt => opt.Topic = channel);            
            sc.AddTransient<IMessageSender<DishesReady>, MemoryQueueSender<DishesReady>>();            
        }

        return sc;
    }

    public static IServiceCollection ConfigWorkshopPersistence(this IServiceCollection sc)
    {
        sc.AddSingleton<IRepository<Order>, MemoryRepository<Order>>();
        return sc;
    }
    
    public static IServiceCollection ConfigWorkshopHostService(this IServiceCollection sc)
    {
        sc.AddHostedService<HeadQuarterListener>();

        return sc;
    } 
}
