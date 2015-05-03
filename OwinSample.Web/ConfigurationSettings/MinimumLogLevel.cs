using ConfigInjector;
using Serilog.Events;

namespace OwinSample.Web.ConfigurationSettings
{
    public class MinimumLogLevel : ConfigurationSetting<LogEventLevel>
    {
    }
}