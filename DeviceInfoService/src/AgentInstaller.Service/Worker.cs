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
                    //DeviceInfo.GetDeviceInfoWMI(_log);

                    // 2. Query Server for update files (basic request)
                    //var httpClient = new HttpClient();
                    //httpClient.BaseAddress = new Uri("https://localhost:7231/api/DeviceAgent/");
                    //httpClient.GetAsync(httpClient.BaseAddress + "PickMe").Wait();

                    // 3. General file and folder interactions.
                    //ProgramFilesUpdater.Main(_config);


                    // New game plan
                    /**
                     * 1. ping server for DeviceInfo.exe program file updates
                     *      - if update then pop up a desktop window for user to accept update (basically hit ok or cancel)
                     *          * segway into Desktop UI implementation for updater/installer (if even possible to combine both)
                     * 2. execute DeviceInfo.exe process to gather specs from machine
                     *      - Service Worker process will need to be able to handle error gracefully from DeviceInfo.exe
                     *      - ideas for data gathering:
                     *          * DeviceInfo.exe gathers data, makes payload in processes' memory, writes to JSON file and service worker process reads JSON file and POSTs it to server.
                     *          * DeviceInfo.exe gahters data, makes payload in processes' memory, and POSTs it to server. (preferred approach; keeps logic consolidated)
                     *          
                     *  Next steps: 
                     
                     *      - Create AgentInstaller.DeviceInfo project that will be responsible for the DeviceInfo.exe
                     *      - Have AgentInstaller.Service envoke DeviceInfo.exe to send payload to server
                     *          * AgentInstaller.Service should handle failures in DeviceInfo.exe and notify server of failure
                     *      - AgentInstaller.Service goals
                     *          * be able to run DeviceInfo.exe
                     *          * be able to update DeviceInfo.exe from server.
                     *              - Currently publishing under .src/BuildTestingGrounds as a series of dll files.
                     *                Might be good for update process if link AgentInstaller.Core as a dll. If we only
                     *                update the AgentInstaller.Core files then AgentInstaller.Service only has to download 1 file
                     *              - 1st idea for checking for updates.
                     *                  - AgentInstaller.Service checks version file on client machine then pings 
                     *                    server for shell structure of AgentInstaller.DeviceInfo files on server 
                     *                    AgentInstaller.Service then checks the shell payload from server and does a meta data check (probably file date modified)
                     *                    against the files on client machine comparing differences. (needs to support additions/removals/updates...basically server knows all and files on client should accept changes from server)
                     *                  - planning on using a "version.json" to handle the updates
                     *                    
                     * **/
                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                }
            }
            catch(Exception e)
            {
                
            }
        }
    }
}
