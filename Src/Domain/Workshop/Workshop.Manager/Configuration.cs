// Copyright (c) Demo.
namespace Workshop;

using Communication;
using Domain.Message;
using Microsoft.Extensions.DependencyInjection;
public static class Configuration
{
    public static IServiceCollection ConfigWorkshopCommunication(this IServiceCollection sc)
    {
        sc.AddSingleton<QueueMessageBroker>();
        sc.AddTransient<IMessageQuerier<DishesScheduled>, MemoryQueueQuerier<DishesScheduled>>();

        return sc;
    }
    
    public static IServiceCollection ConfigWorkshopHostService(this IServiceCollection sc)
    {
        sc.AddHostedService<HeadQuarterListener>();

        return sc;
    } 
}
