namespace HeadQuarter;

using System.Threading.Tasks;
using Delivery;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ordering;
using Workshop;
internal abstract class Program
{
    private static IServiceCollection ConfigHeadQuarter(IServiceCollection sc)
    {
        sc.ConfigHeadQuarterCommandHandler();
        sc.ConfigHeadQuarterCommunication();
        sc.ConfigHeadQuarterHostService();

        return sc;
    }

    private static IServiceCollection ConfigOrdering(IServiceCollection sc)
    {
        sc.ConfigOrderingCommandHandler();
        sc.ConfigOrderingService();
        sc.ConfigOrderingCommunication();
        sc.ConfigOrderingPersistence();
        sc.ConfigOrderingHostService();

        return sc;
    }

    private static IServiceCollection ConfigDelivery(IServiceCollection sc)
    {
        sc.ConfigDeliveryCommunication();
        sc.ConfigDeliveryHostService();
        return sc;
    }

    private static IServiceCollection ConfigWorkshop(IServiceCollection sc)
    {
        sc.ConfigWorkshopCommunication();
        sc.ConfigWorkshopHostService();

        return sc;
    }
    
    public static async Task Main(string[] args)
    {
        var hostBuilder = Host.CreateDefaultBuilder().ConfigureServices((hostContext, services) =>
        {
            ConfigHeadQuarter(services);
            ConfigOrdering(services);
            ConfigDelivery(services);
            ConfigWorkshop(services);
        });

        await hostBuilder.RunConsoleAsync().ConfigureAwait(false);
    }
}
