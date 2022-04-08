using System;
using System.Configuration;
using System.Diagnostics;

namespace Atom.Configuration
{
    internal sealed class ConfigurationProvider : IConfigurationProvider
    {
        private static readonly object Lock;
        private static IConfigurationProvider _current;
        private System.Configuration.Configuration _configuration;

        static ConfigurationProvider()
        {
            Lock = new object();
        }

        private ConfigurationProvider()
        {

        }

        public static IConfigurationProvider Current
        {
            get
            {
                if (_current == null)
                {
                    lock (Lock)
                    {
                        if (_current == null)
                        {
                            ConfigurationProvider current = new ConfigurationProvider();
                            current.Initialize();
                            _current = current;
                        }
                    }
                }
                return _current;
            }
        }

        public ModulesSection Modules { get; private set; }

        private void Initialize()
        {
            try
            {
                string dllConfigPath = GetType().Assembly.Location + ".config";
                ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
                fileMap.ExeConfigFilename = dllConfigPath;
                _configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                Modules = (ModulesSection)_configuration.GetSection("modules");
            }
            catch (Exception exception)
            {
                LoggingProvider.Current.Log(TraceEventType.Critical, exception);
                throw;
            }
        }

    }
}
