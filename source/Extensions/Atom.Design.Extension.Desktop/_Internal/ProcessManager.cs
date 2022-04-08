using Atom.Runtime.Extension.Desktop;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;

namespace Atom.Design.Extension.Desktop
{
    internal static class ProcessManager
    {
        private static Process ResolveProcess(Element element)
        {
            int processId = element.Properties.ProcessId;
            Process process = Process.GetProcessById(processId);
            if (!IsStoreHostProcess(process))
            {
                return process;
            }
            foreach (Element childElement in element)
            {
                process = ResolveProcess(childElement);
                if (process != null)
                {
                    return process;
                }
            }
            return null;
        }

        private static string GetWindowsAppsPath()
        {
            RegistryView registryView = Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32;
            RegistryKey localKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView);
            using (RegistryKey appx = localKey.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Appx"))
            {
                string appsPath = (string)appx.GetValue("PackageRoot");
                return appsPath;
            }
        }

        private static bool IsStoreApplicationProcess(Process process)
        {
            string appsPath = GetWindowsAppsPath();
            string executablePath = GetExecutableFullName(process);
            return executablePath.StartsWith(appsPath, StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsStoreHostProcess(Process process)
        {
            const string hostProcessName = "ApplicationFrameHost";
            return string.Equals(process.ProcessName, hostProcessName, StringComparison.OrdinalIgnoreCase);
        }

        public static Application GetApplicationInformation(Element element)
        {
            Process process = ResolveProcess(element);
            if (IsStoreApplicationProcess(process))
            {
                return GetStoreApplicationInformation(process);
            }
            return GetExecutableApplicationInformation(process);
        }


        public static ExecutableApplication GetExecutableApplicationInformation(Process process)
        {
            string fileFullName = GetExecutableFullName(process);
            ExecutableApplication applicationInfo = new ExecutableApplication(fileFullName);
            return applicationInfo;
        }

        public static StoreApplication GetStoreApplicationInformation(Process process)
        {
            string fileFullName = GetExecutableFullName(process);
            string applicationPath = Path.GetDirectoryName(fileFullName);
            string applicationFolder = Path.GetFileName(applicationPath);
            string[] applicationNameParts = applicationFolder.Split('_');
            string appName = applicationNameParts[0];
            StoreApplication applicationInfo = new StoreApplication(appName);
            return applicationInfo;
        }

        private static string GetExecutableFullName(Process process)
        {
            string queryString = string.Format("SELECT ExecutablePath, CommandLine FROM Win32_Process WHERE ProcessId={0}", process.Id);
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(queryString))
            {
                using (ManagementObjectCollection results = searcher.Get())
                {
                    ManagementObject result = results.Cast<ManagementObject>().First();
                    string executableFullName = (string)result["ExecutablePath"];
                    //string processArguments = (string)result["CommandLine"];
                    return executableFullName;
                }
            }
        }
    }
}
