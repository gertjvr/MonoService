using Autofac;
using OwinSample.Web.Infrastructure.Clock;

namespace OwinSample.Web.AutofacModules
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