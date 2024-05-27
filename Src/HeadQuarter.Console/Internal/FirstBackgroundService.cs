// Copyright (c) Demo.
namespace HeadQuarter;

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
public class FirstBackgroundService : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _ = this.RunBackgroundTask(cancellationToken);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task RunBackgroundTask(CancellationToken cancellationToken)
    {
        var count = 0;
        while (!cancellationToken.IsCancellationRequested)
        {
            // Your first long-running logic here
            Console.WriteLine($"First background service is running... {++count}");

            // Simulate work (replace with your actual logic)
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken).ConfigureAwait(false);
        }
    }
}
