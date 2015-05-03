using System;
using ConfigInjector;

namespace OwinSample.Web.ConfigurationSettings
{
    public class NimbusConnectionString : ConfigurationSetting<string>
    {
        public override string Value
        {
            get { return base.Value.Replace("{MachineName}", Environment.MachineName); }
            set { base.Value = value; }
        }
    }
}