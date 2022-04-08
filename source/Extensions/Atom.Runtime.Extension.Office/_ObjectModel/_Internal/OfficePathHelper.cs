using Microsoft.Win32;
using System;
using System.IO;

namespace Atom.Office
{
    internal static class OfficePathHelper
    {
        public static string GetApplicationFullPath(ApplicationVersion applicationVersion, ApplicationType applicationType)
        {
            string version = StringVersionToApplicationVersionConverter.Convert(applicationVersion);
            string process = ApplicationTypeToProcessNameConverter.Convert(applicationType);
            string officeRegistryEntry = String.Format(@"SOFTWARE\Microsoft\Office\{0}\Common\InstallRoot", version);
            string pathFromRegistry = GetValue(officeRegistryEntry, "Path");
            return Path.Combine(pathFromRegistry, process).ToLower();
        }

        private static string GetValue(string path, string keyName)
        {
            RegistryKey regKey = Registry.LocalMachine.OpenSubKey(path);
            return regKey == null ? string.Empty : regKey.GetValue(keyName).ToString();
        }
    }
}
