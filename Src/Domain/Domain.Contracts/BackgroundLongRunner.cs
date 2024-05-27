// Copyright (c) Demo.
namespace Domain;

using System;
using System.Threading;
using System.Threading.Tasks;
public abstract class BackgroundLongRunner(TimeSpan waitTimeWhenNoJob)
{
    private TimeSpan WaitTimeWhenNoJob { get; } = waitTimeWhenNoJob;
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(() => this.DoWork(cancellationToken), cancellationToken);
        // _ = this.DoWork(cancellationToken);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task DoWork(CancellationToken cancellationToken)
    {
        var count = 0;
        while (!cancellationToken.IsCancellationRequested)
        {
            var doneOnce = await this.DoOnce(count++).ConfigureAwait(false);
            if (!doneOnce)
            {
                // Console.WriteLine($"{nameof(OrderingListener)} is listening...");
                await Task.Delay(this.WaitTimeWhenNoJob, cancellationToken).ConfigureAwait(false);                
            }
        }
    }

    protected abstract Task<bool> DoOnce(int jobCounter);
}
