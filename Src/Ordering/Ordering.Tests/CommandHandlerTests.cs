namespace Ordering;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Communication;
using Core.Command;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ordering.Command;
using Ordering.Message;
using Ordering.Model;
using Ordering.Service;
[TestClass]
public class CommandHandlerTests
{
    private static IServiceProvider Setup(ServiceCollection sc, Action<ServiceCollection> postConfig)
    {
        sc.ConfigOrderingCommandHandler();
        sc.ConfigOrderingService();
        sc.ConfigOrderingCommunication();
        postConfig(sc);

        return sc.BuildServiceProvider();
    }

    private static IEnumerable<Order> Orders
    {
        get
        {
            var address = new Address("street", "LA", "CA", 91100);
            var customer = new Customer(1, "Test", address);
            var order = new Order(1, customer, new List<Dishes>(), new PaymentInfo(), address);

            yield return order;
        }
    }

    private static IEnumerable<IDemoCommand> Commands
    {
        get
        {
            foreach (var order in Orders)
            {
                yield return new AcceptOrder(new VerifyData(order));
                // yield return new CancelOrder(new CancelData()); // not impl
                yield return new RejectOrder(new RejectData(order));
                yield return new SubmitOrder(new SubmitData(order));                
            }
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
        var sp = Setup([], _ => {});
        Assert.IsNotNull(sp);
        var commandBus = sp.GetRequiredService<OrderingCommandBus>();
        var ret = await commandBus.Execute(cmd).ConfigureAwait(false);
        Assert.IsTrue(ret);
    }

    private static IEnumerable<object[]> TestSubmitOrder
    {
        get
        {
            return Orders.Select(o => new []{o});
        }
    } 

    [DataTestMethod]
    [DynamicData(nameof(TestSubmitOrder))]
    
    public async Task TestSubmitOrderToRejectOrder_Succeed(Order order)
    {
        var alwaysRejectPaymentServ = new Mock<IPaymentService>();
        alwaysRejectPaymentServ.Setup(serv => serv.Pay(It.IsAny<int>(), It.IsAny<PaymentInfo>())).Returns(Task.FromResult(false));

        var stubRejectOrderHandler = new Mock<ICommandHandler<RejectOrder>>();
        
        var sp = Setup([], sc =>
        {
            sc.AddSingleton<IPaymentService>(_ => alwaysRejectPaymentServ.Object); // overwrite the default mock payment service to always reject payment;
            sc.AddSingleton<ICommandHandler<RejectOrder>>(_ => stubRejectOrderHandler.Object); // overwrite handler to ensure it's being invoked
        });
        Assert.IsNotNull(sp);
        
        var commandBus = sp.GetRequiredService<OrderingCommandBus>();
        var submitCmd = new SubmitOrder(new SubmitData(order));
        var ret = await commandBus.Execute(submitCmd).ConfigureAwait(false);
        Assert.IsTrue(ret);
        
        stubRejectOrderHandler.Verify(handler => handler.Process(It.IsAny<RejectOrder>()), Times.Once);
    }

    [DataTestMethod]
    [DynamicData(nameof(TestSubmitOrder))]
    public async Task TestSubmitOrderToAcceptOrder_Succeed(Order order)
    {
        var alwaysApprovePaymentServ = new Mock<IPaymentService>();
        alwaysApprovePaymentServ.Setup(serv => serv.Pay(It.IsAny<int>(), It.IsAny<PaymentInfo>())).Returns(Task.FromResult(true));
        
        var sp = Setup([], sc =>
        {
            sc.AddSingleton<IPaymentService>(_ => alwaysApprovePaymentServ.Object); // overwrite the default mock payment service to always approve payment;
            sc.AddTransient<IMessageQuerier<OrderApproved>, MemoryQueueQuerier<OrderApproved>>(); // add a message reader to verify message
        });
        Assert.IsNotNull(sp);
        
        var commandBus = sp.GetRequiredService<OrderingCommandBus>();
        var submitCmd = new SubmitOrder(new SubmitData(order));
        var ret = await commandBus.Execute(submitCmd).ConfigureAwait(false);
        Assert.IsTrue(ret);

        var receiver = sp.GetRequiredService<IMessageQuerier<OrderApproved>>();
        var msg = await receiver.Receive().ConfigureAwait(false);
        Assert.IsNotNull(msg);
    }
}