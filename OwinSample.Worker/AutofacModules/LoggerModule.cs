using Autofac;
using Serilog;

namespace OwinSample.Worker.AutofacModules
{
    public class LoggerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            // we fetch this each time as the instance pointed to by Log.Logger gets changed at startup.
            builder.Register(c => Log.Logger)
                   .As<ILogger>()
                   .ExternallyOwned();
        }
    }
}