using System;
using ConfigInjector.QuickAndDirty;
using OwinSample.Web.ConfigurationSettings;
using OwinSample.Web.Infrastructure.Environments;
using Serilog;
using Serilog.Extras.Topshelf;
using Serilog.Extras.Web.Enrichers;
using Topshelf;

namespace OwinSample.Web
{
    public class Program
    {
        public static int Main(string[] args)
        {
            ConfigureLogging();

            Log.Debug("1");

            var serviceName = DefaultSettingsReader.Get<ServiceName>();
            var serviceDescription = DefaultSettingsReader.Get<ServiceDescription>();

            Log.Debug("2");

            var exitCode = HostFactory.Run(host =>
            {
                host.Service<WebService>();
                host.UseSerilog();
                host.SetDisplayName(serviceDescription);
                host.SetServiceName(serviceName);
                host.RunAsNetworkService();
            });

            return (int) exitCode;
        }

        private static void ConfigureLogging()
        {
            var loggerConfig = new LoggerConfiguration()
                   .MinimumLevel.Is(DefaultSettingsReader.Get<MinimumLogLevel>())
                   .Enrich.FromLogContext()
                   .Enrich.WithMachineName()
                   .Enrich.WithThreadId()
                   .Enrich.WithProperty("ApplicationName", "OwinSample.Web")
                   .Enrich.WithProperty("ApplicationVersion", typeof(IoC).Assembly.GetName().Version)
                   .Enrich.WithProperty("EnvironmentType", AppEnvironment.EnvironmentType)
                   .Enrich.WithProperty("EnvironmentName", AppEnvironment.EnvironmentName)
                   .Enrich.WithProperty("EnvironmentOSVersion", Environment.OSVersion)
                   .Enrich.WithProperty("ServiceAccount", Environment.UserName)
                   .Enrich.With<HttpRequestClientHostIPEnricher>()
                   .Enrich.With<HttpRequestIdEnricher>()
                   .Enrich.With<HttpRequestNumberEnricher>()
                   .Enrich.With<HttpRequestRawUrlEnricher>()
                   .Enrich.With<HttpRequestTraceIdEnricher>()
                   .Enrich.With<HttpRequestTypeEnricher>()
                   .Enrich.With<HttpRequestUrlReferrerEnricher>()
                   .Enrich.With<HttpRequestUserAgentEnricher>()
                   .Enrich.With<UserNameEnricher>()
                   .WriteTo.ColoredConsole()
                   .WriteTo.Seq(DefaultSettingsReader.Get<SeqServerUri>().ToString())
                   .WriteTo.Trace();

            Log.Logger = loggerConfig.CreateLogger();
        }
    }
}
