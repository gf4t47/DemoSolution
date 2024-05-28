namespace Workshop;

using System;
using System.Threading.Tasks;
using Communication;
using Domain;
using Domain.Message;
using Microsoft.Extensions.Hosting;
public class HeadQuarterListener(IMessageQuerier<DishesScheduled> dishReceiver) : BackgroundLongRunner(TimeSpan.FromSeconds(1)), IHostedService
{
    private IMessageQuerier<DishesScheduled> DishReceiver { get; } = dishReceiver;

    protected override async Task<bool> DoOnce(long jobCounter)
    {
        var msg = await this.DishReceiver.Receive().ConfigureAwait(false);
        if (msg is not null)
        {
            var payload = msg.Payload;
            
            Console.WriteLine($"{this.GetType().FullName} recv: {payload.Customer.FullName}@{payload.Customer.Id}, [{string.Join(",", payload.Food)}]");
            return true;
        }

        return false;
    }
}
