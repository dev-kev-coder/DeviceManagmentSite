using System;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using AgentInstaller.Service.utils;
using AgentInstaller.Service.utils.ProgramFilesUpdater;
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
        private readonly IConfiguration _config;

        public Worker(ILogger<Worker> log, IConfiguration config) 
        {
            _log = log;
            _config = config;
        }

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
            try
            {

                _log.LogInformation("AgentInstaller.Service booted at {Time}", DateTimeOffset.Now);

                while (!stoppingToken.IsCancellationRequested)
                {
                    _log.LogInformation("Heartbeat {Time}", DateTimeOffset.Now);


                    // 1. Util to gather device information.
                    //Kernel32.GetNativeSystemInfo(out var info); // Direct DLL implementation
                    DeviceInfo.GetDeviceInfoWMI(_log);

                    // 2. Query Server for update files (basic request)
                    //var httpClient = new HttpClient();
                    //httpClient.BaseAddress = new Uri("https://localhost:7231/api/DeviceAgent/");
                    //httpClient.GetAsync(httpClient.BaseAddress + "PickMe").Wait();

                    // 3. General file and folder interactions.
                    ProgramFilesUpdater.Main(_config);

                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                }
            }
            catch(Exception e)
            {
                
            }
        }
    }
}
