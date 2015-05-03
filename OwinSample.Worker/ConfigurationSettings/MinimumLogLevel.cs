using ConfigInjector;
using Serilog.Events;

namespace OwinSample.Worker.ConfigurationSettings
{
    public class MinimumLogLevel : ConfigurationSetting<LogEventLevel>
    {
    }
}