using Autofac;
using ConfigInjector.Configuration;

namespace OwinSample.Worker.AutofacModules
{
    public class ConfigurationSettingsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            ConfigurationConfigurator.RegisterConfigurationSettings()
                .FromAssemblies(ThisAssembly)
                .RegisterWithContainer(configSetting => builder.RegisterInstance(configSetting)
                    .AsSelf()
                    .SingleInstance())
                .AllowConfigurationEntriesThatDoNotHaveSettingsClasses(true)
                .DoYourThing();
        }
    }
}