using System;
using OwinSample.Web.ConfigurationSettings;

namespace OwinSample.Web.Infrastructure.Environments
{
    public static class AppEnvironment
    {
        private static bool _isInitialized;
        private static bool _isSimulating;

        private static string _machineName;
        private static EnvironmentType? _environmentType;
        private static EnvironmentName? _environmentName;

        private static string _simulatedMachineName;
        private static EnvironmentType? _simulatedEnvironmentType;
        private static EnvironmentName? _simulatedEnvironmentName;

        public static string MachineName
        {
            get
            {
                if (_isInitialized == false) Initialize();
                return _isSimulating ? _simulatedMachineName : _machineName;
            }
        }

        public static EnvironmentType EnvironmentType
        {
            get
            {
                if (_isInitialized == false) Initialize();
                return _isSimulating ? _simulatedEnvironmentType.Value : _environmentType.Value;
            }
        }

        public static EnvironmentName EnvironmentName
        {
            get
            {
                if (_isInitialized == false) Initialize();
                return _isSimulating ? _simulatedEnvironmentName.Value : _environmentName.Value;
            }
        }

        public static bool IsLocal()
        {
            return EnvironmentType == EnvironmentType.Local;
        }

        public static bool IsProduction()
        {
            return EnvironmentType == EnvironmentType.Production;
        }

        public static bool IsTest()
        {
            return EnvironmentType == EnvironmentType.Test;
        }

        public static void Simulate(string machineName, EnvironmentType? type, EnvironmentName? name)
        {
            _simulatedMachineName = machineName;
            _simulatedEnvironmentType = type;
            _simulatedEnvironmentName = name;
            _isSimulating = true;
        }

        static void Initialize()
        {
            // Skip initialization if we are simulating
            if (_isSimulating) return;

            _machineName = Environment.MachineName;

            _environmentType = ConfigInjector.QuickAndDirty.DefaultSettingsReader.Get<EnvironmentTypeSetting>();
            _environmentName = ConfigInjector.QuickAndDirty.DefaultSettingsReader.Get<EnvironmentNameSetting>();

            _isInitialized = true;
        }

        public static void Reset()
        {
            _isInitialized = false;
            _isSimulating = false;
            _machineName = null;
            _simulatedMachineName = null;
            _environmentType = null;
            _simulatedEnvironmentType = null;
            _environmentName = null;
            _simulatedEnvironmentName = null;
        }
    }
}