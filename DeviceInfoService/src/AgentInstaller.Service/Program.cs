//using System;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Hosting.WindowsServices;

namespace AgentInstaller.Service
{
    public class Program
    {
        // entry point — Windows Service **or** Console depending on how it’s launched
        public static void Main(string[] args)
        {
            // Build and run the generic host
            var host = CreateHostBuilder(args);

            host.Build().Run();
        }
        private static HostApplicationBuilder CreateHostBuilder(string[] args)
        {
            var appBuilderSettings = new HostApplicationBuilderSettings();
            // Command-line flags (automatic --key=value flag parsing)
            appBuilderSettings.Args = args; 

            // Path for relative file look ups (if the process needs to read from or write to a file?)
            appBuilderSettings.ContentRootPath = Directory.GetCurrentDirectory();

            // Sets the EnvironmentName for the host process
            // I believe this can later be used in injected services to be able to tell what run time the host process is currently running in
            // Useful for swapping between Dev testing and Prod ready programs
            // Literally this is just a way to configure your hosts process and change any settings that does not affect implementation
            appBuilderSettings.EnvironmentName = Environments.Development; 

            // App name will show up on logger. Helpful for libraries to locate resources
            appBuilderSettings.ApplicationName = "DeviceInfoAgent";

            // Builder to use to configure Host
            var appBuilder = Host
                .CreateEmptyApplicationBuilder(appBuilderSettings);
           
            // Not really sure what this does yet
            appBuilder.Configuration
                .AddJsonFile($"appsettings.{appBuilderSettings.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args);

            // Not really sure what this does yet
            appBuilder.Logging
                .AddConsole()
                .AddDebug()
                .SetMinimumLevel(LogLevel.Information);

            // Not really sure what this does yet either...
            appBuilder.Services
                .AddHostedService<Worker>()
                .AddWindowsService();

            return appBuilder;
        }
    }
}
