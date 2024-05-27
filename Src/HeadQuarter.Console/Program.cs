namespace HeadQuarter;

using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ordering;
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

        return sc;
    }
    
    public static async Task Main(string[] args)
    {
        var hostBuilder = Host.CreateDefaultBuilder().ConfigureServices((hostContext, services) =>
        {
            ConfigHeadQuarter(services);
            ConfigOrdering(services);
        });

        await hostBuilder.RunConsoleAsync().ConfigureAwait(false);
    }
}
