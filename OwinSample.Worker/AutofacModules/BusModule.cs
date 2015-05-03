using System;
using Autofac;
using Nimbus;
using Nimbus.Configuration;
using Nimbus.Infrastructure;
using Nimbus.Logger.Serilog;
using OwinSample.MessageContracts.Requests;
using OwinSample.Worker.ConfigurationSettings;

namespace OwinSample.Worker.AutofacModules
{
    public class BusModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var messageContractsAssembly = typeof(AuthenticateUserRequest).Assembly;
            var handlerTypesProvider = new AssemblyScanningTypeProvider(ThisAssembly, messageContractsAssembly);

            builder.RegisterType<SerilogStaticLogger>()
                .As<ILogger>()
                .SingleInstance();

            builder.RegisterNimbus(handlerTypesProvider);
            builder.Register(c => new BusBuilder()
                                        .Configure()
                                        .WithLogger(c.Resolve<ILogger>())
                                        .WithConnectionString(c.Resolve<NimbusConnectionString>())
                                        .WithNames("OwinSample.Worker", Environment.MachineName)
                                        .WithTypesFrom(handlerTypesProvider)
                                        .WithDefaultConcurrentHandlerLimit(1024)
                                        .WithJsonSerializer()
                                        //.WithGlobalOutboundInterceptorTypes(typeof(OutboundAuditingInterceptor))
                                        .WithAutofacDefaults(c)
                                        .WithHeartbeatInterval(c.Resolve<DefaultBusHeartbeatInterval>())
                                        .Build())
                .As<IBus>()
                .SingleInstance();
        }
    }
}