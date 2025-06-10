using System;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AgentInstaller.Service;
/**
 * Link below is Microsofts guidance on building Work services using .NET
 * https://learn.microsoft.com/en-us/dotnet/core/extensions/workers
 * **/
public sealed class Worker : BackgroundService
{
    private readonly ILogger<Worker> _log;

    public Worker(ILogger<Worker> log) => _log = log;

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public override void Dispose()
    {
        base.Dispose();
    }

    /// <summary>
    /// Triggered when the application host is ready to start the service.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        return base.StartAsync(cancellationToken);
    }

    /// <summary>
    /// Triggered when the application host is performing a graceful shutdown.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override Task StopAsync(CancellationToken cancellationToken)
    {
        return base.StopAsync(cancellationToken);
    }

    /// <summary>
    /// 
    /// This method is called when the IHostedService starts. 
    /// The implementation should return a task that represents the lifetime of the long running operation(s) being performed.
    /// 
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //_log.LogInformation("AgentInstaller.Service booted at {Time}", DateTimeOffset.Now);

        while (!stoppingToken.IsCancellationRequested)
        {
            // TODO: real work—ping server, collect telemetry, etc.
            _log.LogInformation("Heartbeat {Time}", DateTimeOffset.Now);
            await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        }
    }
}
