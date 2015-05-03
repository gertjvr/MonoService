using Autofac;
using Newtonsoft.Json;
using OwinSample.Worker.Infrastructure;

namespace OwinSample.Worker.AutofacModules
{
    public class JsonSerializerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterInstance<JsonSerializer>(new CustomJsonSerializer())
                .SingleInstance();
        }
    }
}