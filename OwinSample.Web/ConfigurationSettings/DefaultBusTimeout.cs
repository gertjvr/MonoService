using System;
using ConfigInjector;

namespace OwinSample.Web.ConfigurationSettings
{
    public class DefaultBusTimeout : ConfigurationSetting<TimeSpan>
    {
    }
}