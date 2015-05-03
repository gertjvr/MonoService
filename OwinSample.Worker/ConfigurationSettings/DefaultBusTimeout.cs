using System;
using ConfigInjector;

namespace OwinSample.Worker.ConfigurationSettings
{
    public class DefaultBusTimeout : ConfigurationSetting<TimeSpan>
    {
    }
}