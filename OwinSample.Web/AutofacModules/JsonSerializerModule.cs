using Autofac;
using Newtonsoft.Json;
using OwinSample.Web.Infrastructure;

namespace OwinSample.Web.AutofacModules
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