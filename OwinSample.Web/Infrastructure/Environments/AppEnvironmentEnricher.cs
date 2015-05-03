using Serilog.Core;
using Serilog.Events;

namespace OwinSample.Web.Infrastructure.Environments
{
    /// <summary>
    /// Enriches log events with the AppEnvironment details.
    /// </summary>
    public class AppEnvironmentEnricher : ILogEventEnricher
    {
        public const string MachineNamePropertyName = "MachineName";
        public const string EnvironmentTypePropertyName = "EnvironmentType";
        public const string EnvironmentNamePropertyName = "EnvironmentName";
        public const string ApplicationNamePropertyName = "ApplicationName";
        public const string ApplicationVersionPropertyName = "ApplicationVersion";

        readonly string _applicationName;
        readonly string _applicationVersion;
        private LogEventProperty[] _cachedProperties;

        public AppEnvironmentEnricher(string applicationName, string applicationVersion)
        {
            _applicationName = applicationName;
            _applicationVersion = applicationVersion;
        }

        /// <summary>
        /// Enrich the log event.
        /// 
        /// </summary>
        /// <param name="logEvent">The log event to enrich.</param><param name="propertyFactory">Factory for creating new properties to add to the event.</param>
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (_cachedProperties == null)
            {
                _cachedProperties = new []
                {
                    propertyFactory.CreateProperty(MachineNamePropertyName, AppEnvironment.MachineName, destructureObjects: false),
                    propertyFactory.CreateProperty(EnvironmentTypePropertyName, AppEnvironment.EnvironmentType, destructureObjects: false),
                    propertyFactory.CreateProperty(EnvironmentNamePropertyName, AppEnvironment.EnvironmentName, destructureObjects: false),
                    propertyFactory.CreateProperty(ApplicationNamePropertyName, _applicationName, destructureObjects: false),
                    propertyFactory.CreateProperty(ApplicationVersionPropertyName, _applicationVersion, destructureObjects: false),
                };
            }

            foreach (var property in _cachedProperties)
            {
                logEvent.AddPropertyIfAbsent(property);
            }
        }
    }
}