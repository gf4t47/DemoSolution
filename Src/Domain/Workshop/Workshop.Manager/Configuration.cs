// Copyright (c) Demo.
namespace Workshop;

using Communication;
using Core.Command;
using Domain.Message;
using Microsoft.Extensions.DependencyInjection;
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
        sc.AddTransient<IMessageQuerier<DishesScheduled>, MemoryQueueQuerier<DishesScheduled>>();
        sc.AddTransient<IMessageSender<DishesReady>, MemoryQueueSender<DishesReady>>();

        return sc;
    }
    
    public static IServiceCollection ConfigWorkshopHostService(this IServiceCollection sc)
    {
        sc.AddHostedService<HeadQuarterListener>();

        return sc;
    } 
}
