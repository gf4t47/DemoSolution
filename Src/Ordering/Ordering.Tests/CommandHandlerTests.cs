namespace Ordering;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Command;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ordering.Command;
using Ordering.Model;
[TestClass]
public class CommandHandlerTests
{
    private IServiceProvider? ServiceProvider { get; set; }
    
    [TestInitialize]
    public void Setup()
    {
        var sc = new ServiceCollection();
        sc.ConfigOrderingCommandHandler();
        sc.ConfigOrderingService();

        this.ServiceProvider = sc.BuildServiceProvider();
    }

    private static IEnumerable<IDemoCommand> Commands
    {
        get
        {
            var address = new Address("street", "LA", "CA", 91100);
            var customer = new Customer(1, "Test", address);
            var order = new Order(1, customer, new List<Dishes>(), new PaymentInfo(), address);
            
            yield return new AcceptOrder(new VerifyData(order));
            // yield return new CancelOrder(new CancelData()); // not impl
            yield return new RejectOrder(new RejectData(order));
            yield return new SubmitOrder(new SubmitData(order));
        }
    }

    private static IEnumerable<object[]> TestExecCommandSucceed
    {
        get
        {
            return Commands.Select(c => new []{c});    
        }
    }

    [DataTestMethod]
    [DynamicData(nameof(TestExecCommandSucceed))]
    public async Task TestExecCommand_Succeed(IDemoCommand cmd)
    {
        Assert.IsNotNull(this.ServiceProvider);
        var commandBus = this.ServiceProvider.GetRequiredService<OrderingCommandBus>();
        var ret = await commandBus.Execute(cmd);
        Assert.IsTrue(ret);
    }
}