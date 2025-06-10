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
            // DLL library
            //Console.WriteLine(AgentInstaller.Core.Configuration.AgentOptions.SayHello());

            // Build and run the generic host
            var host = CreateHostBuilder(args);

            host.Build().Run();
        }
        //private static IHostBuilder CreateHostBuilder(string[] args)
        private static HostApplicationBuilder CreateHostBuilder(string[] args)
        {
            //var hostBuilder = Host.CreateDefaultBuilder();
            //hostBuilder.ConfigureServices(services =>
            //{
            //    services.AddHostedService<Worker>();
            //});

            //return hostBuilder;
            var appBuilderSettings = new HostApplicationBuilderSettings();
            appBuilderSettings.ApplicationName = "DeviceInfoAgent";
            //appBuilderSettings.ContentRootPath = Directory.GetCurrentDirectory();
            var builder = Host.CreateEmptyApplicationBuilder(appBuilderSettings);
            builder.Services.AddLogging(log => { log.AddConsole(); log.SetMinimumLevel(LogLevel.Information); });
            builder.Services.AddHostedService<Worker>();
            return builder;
        }

        /// <summary>
        /// Configures the Host for console AND Windows-Service use.
        /// </summary>
        //private static IHostBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateDefaultBuilder(args)
        //        // ❗ tell the runtime this exe can run as a service
        //        .UseWindowsService(options =>
        //        {
        //            // (visible name in services.msc)
        //            options.ServiceName = "AgentInstaller Service";
        //        })
        //        // add logging, configuration, DI, etc. here
        //        .ConfigureServices((hostContext, services) =>
        //        {
        //            services.AddHostedService<Worker>();      // the background worker you scaffolded
        //            // services.AddSingleton<IMyDependency, MyDependency>();
        //        })
        //        // optional: tweak default logging
        //        .ConfigureLogging(logging =>
        //        {
        //            logging.ClearProviders();
        //            logging.AddConsole();
        //        });
    }
}
