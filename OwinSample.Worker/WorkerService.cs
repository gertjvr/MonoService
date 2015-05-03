using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Autofac;
using Nimbus;
using Nimbus.Configuration;
using Serilog;
using Topshelf;

namespace OwinSample.Worker
{
    public class WorkerService : ServiceControl
    {
        private IContainer _container;

        public bool Start(HostControl hostControl)
        {
            try
            {
                Log.Debug("Starting OwinSample.Worker...");

                HookUpToUnhandledExceptionsInTheAppDomain();
                HookUpToUnobservedTaskExceptions();

                var sw = Stopwatch.StartNew();

                Log.Debug("Building the container...");
                _container = IoC.LetThereBeIoC();

                StartNimbus((Bus)_container.Resolve<IBus>());

                sw.Stop();
                Log.Information("Started OwinSample.Worker in {StartupTime}", sw.Elapsed);
            }
            catch (Exception e)
            {
                if (Debugger.IsAttached) Debugger.Break();
                Log.Error(e, "Failed to start the Worker: {Message}", e.Message);
                throw;
            }

            Log.Debug("OwinSample.Worker started...");

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            Log.Debug("Stoping OwinSample.Worker...");

            if (_container == null) return true;

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

            Log.Debug("OwinSample.Worker stopped...");

            return true;
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
    }
}