// Copyright (c) Demo.
namespace Ordering;

using Communication;
using Core.Command;
using Domain;
using Domain.Message;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Command;
using Ordering.Handler;
using Ordering.Model;
using Ordering.Service;
using Persistence;
public static class Configuration
{
    public static IServiceCollection ConfigOrderingCommandHandler(this IServiceCollection sc)
    {
        sc.AddTransient<ICommandHandler<AcceptOrder>, AcceptOrderHandler>();
        sc.AddTransient<ICommandHandler<CancelOrder>, CancelOrderHandler>();
        sc.AddTransient<ICommandHandler<RejectOrder>, RejectOrderHandler>();
        sc.AddTransient<ICommandHandler<SubmitOrder>, SubmitOrderHandler>();

        sc.AddSingleton<OrderingCommandBus>();

        return sc;
    }

    public static IServiceCollection ConfigOrderingService(this IServiceCollection sc)
    {
        sc.AddTransient<IPaymentService, MockPaymentService>();
        sc.AddTransient<ICalcPriceService, MockPriceCalculator>();

        return sc;
    }

    public static IServiceCollection ConfigOrderingCommunication(this IServiceCollection sc)
    {
        sc.AddSingleton<QueueMessageBroker>();

        var (name, channel) = typeof(OrderApproved).ResolveMessageChannel();
        sc.Configure<MessageChannel>(name, opt => opt.Topic = channel);
        sc.AddTransient<IMessageSender<OrderApproved>, MemoryQueueSender<OrderApproved>>();

        return sc;
    }

    public static IServiceCollection ConfigOrderingPersistence(this ServiceCollection sc)
    {
        sc.AddSingleton<IRepository<Order>, MemoryRepository<Order>>();
        sc.AddSingleton<IUniqueIdGenerator<Order>, MemoryRepository<Order>>();

        return sc;
    }
}
