using Autofac;
using OwinSample.Worker.Infrastructure.Clock;

namespace OwinSample.Worker.AutofacModules
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<SystemClock>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}