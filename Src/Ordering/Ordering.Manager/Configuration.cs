// Copyright (c) Demo.
namespace Ordering;

using Communication;
using Core.Command;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Command;
using Ordering.Handler;
using Ordering.Message;
using Ordering.Service;
public static class Configuration
{
    private const string OrderingToHeadQuarters = "Ordering=>HeadQuarters";
    
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
        sc.AddOptions<MessageChannel>().PostConfigure(opt => opt.Topic = OrderingToHeadQuarters);
        sc.AddTransient<IMessageSender<OrderApproved>, MemoryQueueSender<OrderApproved>>();

        return sc;
    }
}
