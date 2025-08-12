using System;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using AgentInstaller.Service.utils;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AgentInstaller.Service
{
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
            var httpClient = new HttpClient();

            httpClient.BaseAddress = new Uri("https://localhost:7231/api/DeviceAgent/");

            try
            {
                var stop = "here";

                //_log.LogInformation("AgentInstaller.Service booted at {Time}", DateTimeOffset.Now);

                while (!stoppingToken.IsCancellationRequested)
                {
                    // TODO: real work—ping server, collect telemetry, etc.
                    _log.LogInformation("Heartbeat {Time}", DateTimeOffset.Now);
                    //Kernel32.GetNativeSystemInfo(out var info);
                    //Console.WriteLine($"{info.dwNumberOfProcessors} logical procs, page size {info.dwPageSize} bytes, arch {info.wProcessorArchitecture}");
                    DeviceInfo.GetDeviceInfoWMI(_log);
                    httpClient.GetAsync(httpClient.BaseAddress + "PickMe").Wait();
                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                }
            }
            catch(Exception e)
            {
                
            }
        }
    }
}
