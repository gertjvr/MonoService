using System;
using ConfigInjector.QuickAndDirty;
using OwinSample.Worker.ConfigurationSettings;
using OwinSample.Worker.Infrastructure.Environments;
using Serilog;
using Serilog.Extras.Topshelf;
using Topshelf;

namespace OwinSample.Worker
{
    public class Program
    {
        private static int Main(string[] args)
        {
            ConfigureLogging();

            var serviceName = DefaultSettingsReader.Get<ServiceName>();
            var serviceDescription = DefaultSettingsReader.Get<ServiceDescription>();

            var exitCode = HostFactory.Run(host =>
            {
                host.Service<WorkerService>();
                host.UseSerilog();
                host.SetDisplayName(serviceDescription);
                host.SetServiceName(serviceName);
                //host.RunAsNetworkService();
            });

            return (int)exitCode;
        }

        private static void ConfigureLogging()
        {
            var loggerConfig = new LoggerConfiguration()
                   .MinimumLevel.Is(DefaultSettingsReader.Get<MinimumLogLevel>())
                   .Enrich.FromLogContext()
                   .Enrich.WithMachineName()
                   .Enrich.WithThreadId()
                   .Enrich.WithProperty("ApplicationName", "OwinSample.Worker")
                   .Enrich.WithProperty("ApplicationVersion", typeof(IoC).Assembly.GetName().Version)
                   .Enrich.WithProperty("EnvironmentType", AppEnvironment.EnvironmentType)
                   .Enrich.WithProperty("EnvironmentName", AppEnvironment.EnvironmentName)
                   .Enrich.WithProperty("EnvironmentOSVersion", Environment.OSVersion)
                   .Enrich.WithProperty("ServiceAccount", Environment.UserName)
                   .WriteTo.ColoredConsole()
                   .WriteTo.Seq(DefaultSettingsReader.Get<SeqServerUri>().ToString())
                   .WriteTo.Trace();

            Log.Logger = loggerConfig.CreateLogger();
        }
    }
}