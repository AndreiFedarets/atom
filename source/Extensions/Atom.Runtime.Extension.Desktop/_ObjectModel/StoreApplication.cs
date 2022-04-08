using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Atom.Runtime.Extension.Desktop
{
    public sealed class StoreApplication : Application
    {
        public StoreApplication(string appName)
        {
            AppName = appName;
        }

        public string AppName { get; private set; }

        public override Process Start()
        {
            string appId = GetApplicationId();
            ApplicationActivationManager appActiveManager = new ApplicationActivationManager();
            uint processId;
            appActiveManager.ActivateApplication(appId, null, ActivateOptions.None, out processId);
            return Process.GetProcessById((int)processId);
        }

        private string GetApplicationId()
        {
            DirectoryInfo windowsAppsDirectory = GetWindowsAppsDirectory();
            DirectoryInfo[] appsDirectories = windowsAppsDirectory.GetDirectories(AppName + "*");
            string directoryName = appsDirectories[0].Name;
            string[] directoryParts = directoryName.Split('_');
            string appId = string.Concat(directoryParts.First(), "_", directoryParts.Last(), "!App");
            return appId;
        }

        private DirectoryInfo GetWindowsAppsDirectory()
        {
            RegistryView registryView = System.Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32;
            RegistryKey localKey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, registryView);
            using (RegistryKey appx = localKey.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Appx"))
            {
                string appsPath = (string)appx.GetValue("PackageRoot");
                return new DirectoryInfo(appsPath);
            }
        }


        internal enum ActivateOptions
        {
            None = 0x00000000,  // No flags set
            DesignMode = 0x00000001,  // The application is being activated for design mode, and thus will not be able to
                                      // to create an immersive window. Window creation must be done by design tools which
                                      // load the necessary components by communicating with a designer-specified service on
                                      // the site chain established on the activation manager.  The splash screen normally
                                      // shown when an application is activated will also not appear.  Most activations
                                      // will not use this flag.
            NoErrorUI = 0x00000002,  // Do not show an error dialog if the app fails to activate.                                
            NoSplashScreen = 0x00000004,  // Do not show the splash screen when activating the app.
        }

        [ComImport, Guid("2e941141-7f97-4756-ba1d-9decde894a3d"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IApplicationActivationManager
        {
            // Activates the specified immersive application for the "Launch" contract, passing the provided arguments
            // string into the application.  Callers can obtain the process Id of the application instance fulfilling this contract.
            IntPtr ActivateApplication([In] String appUserModelId, [In] String arguments, [In] ActivateOptions options, [Out] out UInt32 processId);

            IntPtr ActivateForFile([In] String appUserModelId, [In] IntPtr /*IShellItemArray* */ itemArray, [In] String verb, [Out] out UInt32 processId);

            IntPtr ActivateForProtocol([In] String appUserModelId, [In] IntPtr /* IShellItemArray* */itemArray, [Out] out UInt32 processId);
        }

        [ComImport, Guid("45BA127D-10A8-46EA-8AB7-56EA9078943C")]//Application Activation Manager
        private class ApplicationActivationManager : IApplicationActivationManager
        {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)/*, PreserveSig*/]
            public extern IntPtr ActivateApplication([In] String appUserModelId, [In] String arguments, [In] ActivateOptions options, [Out] out UInt32 processId);

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            public extern IntPtr ActivateForFile([In] String appUserModelId, [In] IntPtr /*IShellItemArray* */ itemArray, [In] String verb, [Out] out UInt32 processId);

            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            public extern IntPtr ActivateForProtocol([In] String appUserModelId, [In] IntPtr /* IShellItemArray* */itemArray, [Out] out UInt32 processId);
        }
    }
}
