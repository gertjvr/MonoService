using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Autofac;
using Nancy;
using Nancy.Owin;
using Nimbus;
using Nimbus.Configuration;
using Owin;
using OwinSample.Web.ConfigurationSettings;
using OwinSample.Web.Infrastructure.ErrorHandling;
using OwinSample.Web.Infrastructure.Extentions;
using OwinSample.Web.Infrastructure.JwtTokenAuthentication;
using OwinSample.Web.Infrastructure.LatencyMonkey;
using Serilog;

namespace OwinSample.Web
{
    public class Startup
    {
        private IContainer _container;

        public void Configuration(IAppBuilder app)
        {
            Log.Debug("Starting OwinSample.Web...");

            HookUpToUnhandledExceptionsInTheAppDomain();
            HookUpToUnobservedTaskExceptions();

            _container = IoC.LetThereBeIoC();

            StartNimbus((Bus)_container.Resolve<IBus>());

            app.UseSerilogErrorHandling();

            if (_container.Resolve<LatencyMonkeyEnabled>()) app.UseLatencyMonkey();

            app.UseJwtTokenAuthentication(
                _container.Resolve<IssuerSetting>(),
                _container.Resolve<AudienceSetting>(),
                _container.Resolve<ClientSecretSetting>());

            app.UseNancy(options =>
            {
                var bootstrapper = new Bootstrapper()
                    .UseContainer(_container)
                    .EnforceHttps(_container.Resolve<RequireTLS>())
                    .ResolveClaimsPrincipal(ResolveClaimsPrincipal);

                options.Bootstrapper = bootstrapper;
                options.PassThroughWhenStatusCodesAre(HttpStatusCode.NotFound);
            });

            app.HookDisposal(TearDown);

            Log.Debug("OwinSample.Web started...");
        }

        private void TearDown()
        {
            Log.Debug("Stoping OwinSample.Web...");

            if (_container == null) return;

            try
            {
                StopNimbus((Bus)_container.Resolve<IBus>());

                Log.Information("Trying to dispose the Autofac Container...");
                _container.Dispose();
            }
            catch (Exception e)
            {
                Log.Error(e, "Failed to dispose the Autofac Container: {Message}", e.Message);
            }

            Log.Debug("OwinSample stopped...");

            _container = null;
        }

        private void StartNimbus(Bus bus)
        {
            Log.Debug("Trying to start Nimbus...");
            try
            {
                // Just wait for our Response message pump to start so we can handle response messages to our own requests
                bus.Start(MessagePumpTypes.Response).Wait();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Failed trying to start Nimbus: {Message}.", e.Message);
                throw;
            }
        }

        private void StopNimbus(Bus bus)
        {
            if (bus == null) return;

            try
            {
                Log.Debug("Trying to stop Nimbus...");
                bus.Stop().Wait();
            }
            catch (Exception e)
            {
                Log.Error(e, "Could not stop Nimbus: {Message}", e.Message);
            }
        }

        private static void HookUpToUnhandledExceptionsInTheAppDomain()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                try
                {
                    Log.Error((Exception)args.ExceptionObject,
                              "Unhandled Exception: {Message}",
                              ((Exception)args.ExceptionObject).Message);
                }
                // ReSharper disable once UnusedVariable
                catch (Exception e)
                {
                    if (Debugger.IsAttached) Debugger.Break();
                }
            };
        }

        private static void HookUpToUnobservedTaskExceptions()
        {
            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                try
                {
                    Log.Error(args.Exception,
                              "Unhandled Exception: {Message}",
                              (args.Exception).Message);
                }
                // ReSharper disable once UnusedVariable
                catch (Exception e)
                {
                    if (Debugger.IsAttached) Debugger.Break();
                }
            };
        }

        private ClaimsPrincipal ResolveClaimsPrincipal(NancyContext context)
        {
            var owinEnvironment = context.GetOwinEnvironment();
            var user = owinEnvironment["server.User"] as ClaimsPrincipal;
            return user;
        }
    }
}