namespace Core;

using System.Threading.Tasks;
using Core.Command;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
[TestClass]
public class DependencyInjectionTests
{
    [TestMethod]
    public async Task TestResolveDI()
    {
        var cmd = new Mock<IDemoCommand<string>>();
        cmd.SetupGet(c => c.Data).Returns("Mock string command");
        
        var handler = new Mock<ICommandHandler<IDemoCommand<string>>>();
        handler.Setup(it => it.Process(It.IsAny<IDemoCommand<string>>())).Returns(Task.FromResult(true));
        
        var sc = new ServiceCollection();
        sc.AddTransient<ICommandHandler<IDemoCommand<string>>>(_ => handler.Object);
        sc.AddSingleton<CommandBus>();

        var provider = sc.BuildServiceProvider();
        var bus = provider.GetRequiredService<CommandBus>();

        var actual = await bus.Execute(cmd.Object).ConfigureAwait(false);
        Assert.IsTrue(actual);
    }
}
