namespace HeadQuarter;

using Communication;
using Domain.Message;
using Microsoft.Extensions.DependencyInjection;

public static class Configuration
{
    private const string OrderingToHeadQuarters = "Ordering=>HeadQuarters";
    
    public static IServiceCollection ConfigHeadQuarterCommunication(this IServiceCollection sc)
    {
        sc.AddSingleton<QueueMessageBroker>();
        sc.AddOptions<MessageChannel>().PostConfigure(opt => opt.Topic = OrderingToHeadQuarters);
        sc.AddTransient<IMessageQuerier<OrderApproved>, MemoryQueueQuerier<OrderApproved>>();

        return sc;
    }
}
