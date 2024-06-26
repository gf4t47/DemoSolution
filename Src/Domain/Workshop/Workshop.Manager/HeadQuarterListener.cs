﻿namespace Workshop;

using System;
using System.Threading.Tasks;
using Communication;
using Domain;
using Domain.Message;
using Microsoft.Extensions.Hosting;
using Workshop.Command;
public class HeadQuarterListener(IMessageQuerier<DishesScheduled> headQuarterReceiver, WorkshopCommandBus commandBus) : BackgroundLongRunner(TimeSpan.FromSeconds(1)), IHostedService
{
    private IMessageQuerier<DishesScheduled> HeadQuarterReceiver { get; } = headQuarterReceiver;
    
    private WorkshopCommandBus CommandBus { get; } = commandBus;

    protected override async Task<bool> DoOnce(long jobCounter)
    {
        var msg = await this.HeadQuarterReceiver.Receive().ConfigureAwait(false);
        if (msg is not null)
        {
            var payload = msg.Payload;
            Console.WriteLine($"{this.GetType().FullName} recv: {payload.Customer.FullName}@{payload.OrderId}, [{string.Join(",", payload.Food)}]");
            
            var data = new CookingData(payload.OrderId, payload.Customer, payload.Food);
            var command = new Cooking(data);
            await this.CommandBus.Execute(command).ConfigureAwait(false);
            
            return true;
        }

        return false;
    }
}
