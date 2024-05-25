// Copyright (c) Demo.
namespace Ordering;

using Core.Command;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Command;
using Ordering.Handler;
using Ordering.Service;
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
}
