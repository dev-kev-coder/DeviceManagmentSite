using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting.WindowsServices;

namespace AgentInstaller.Service
{
    public class Program
    {
        // entry point — Windows Service **or** Console depending on how it’s launched
        public static void Main(string[] args)
        {
            // Build and run the generic host
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Configures the Host for console AND Windows-Service use.
        /// </summary>
        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                // ❗ tell the runtime this exe can run as a service
                .UseWindowsService(options =>
                {
                    // (visible name in services.msc)
                    options.ServiceName = "AgentInstaller Service";
                })
                // add logging, configuration, DI, etc. here
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();      // the background worker you scaffolded
                    // services.AddSingleton<IMyDependency, MyDependency>();
                })
                // optional: tweak default logging
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                });
    }
}
