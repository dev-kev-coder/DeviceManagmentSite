using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AgentInstaller.Service;

public sealed class Worker : BackgroundService
{
    private readonly ILogger<Worker> _log;

    public Worker(ILogger<Worker> log) => _log = log;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _log.LogInformation("AgentInstaller.Service booted at {Time}", DateTimeOffset.Now);

        while (!stoppingToken.IsCancellationRequested)
        {
            // TODO: real work—ping server, collect telemetry, etc.
            _log.LogInformation("Heartbeat {Time}", DateTimeOffset.Now);
            await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
        }
    }
}
