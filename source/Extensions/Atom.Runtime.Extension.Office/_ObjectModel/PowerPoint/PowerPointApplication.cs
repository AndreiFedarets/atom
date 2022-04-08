using Atom.Office.Win32;
using System;
using System.Diagnostics;
using System.Threading;

namespace Atom.Office.PowerPoint
{
    internal class PowerPointApplication : IPowerPointApplication
    {
        private Microsoft.Office.Interop.PowerPoint._Application _powerPointApplication;
        private readonly ApplicationVersion _version;
        private readonly Process _powerPointProcess;

        public const string MainWindowClassName = "PPTFrameClass";
        public const string DocumentWindowClassName = "paneClassDC";

        public PowerPointApplication(Process powerPointProcess, ApplicationVersion version)
        {
            _powerPointProcess = powerPointProcess;
            _version = version;
        }

        public bool Visible
        {
            get
            {
                Initialize();
                return _powerPointApplication.Visible == Microsoft.Office.Core.MsoTriState.msoTrue;
            }
        }

        public ApplicationType Type
        {
            get
            {
                Initialize();
                return ApplicationType.PowerPoint;
            }
        }

        public ApplicationVersion Version
        {
            get { return _version; }
        }

        public IntPtr WindowHandle
        {
            get
            {
                Initialize();
                return _powerPointProcess.MainWindowHandle;
            }
        }
        
        public void Activate()
        {
            Initialize();
            NativeMethods.BringWindowToTop(WindowHandle);
        }

        public bool IsStarted
        {
            get
            {
                if (_powerPointProcess.HasExited)
                {
                    return false;
                }
                IntPtr mainWindowHandle = Process.GetProcessById(_powerPointProcess.Id).MainWindowHandle;
                if (mainWindowHandle != IntPtr.Zero)
                {
                    IntPtr childHandle = NativeMethods.GetFirstChild(mainWindowHandle, DocumentWindowClassName);
                    if (childHandle != IntPtr.Zero)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public void Initialize()
        {
            if (!IsValid)
            {
                _powerPointApplication = ApplicationProvider.FindPowerPointApplication(Process.GetProcessById(_powerPointProcess.Id).MainWindowHandle);
            }
        }

        public bool IsValid
        {
            get
            {
                return _powerPointApplication != null;
            }
        }

        public void Close()
        {
            foreach (var presentation in _powerPointApplication.Presentations)
            {
                (presentation as Microsoft.Office.Interop.PowerPoint.Presentation).Close();
            }
            _powerPointProcess.CloseMainWindow();
            //_powerPointApplication.Quit();
            Dispose();
            while (!_powerPointProcess.HasExited)
            {
                Thread.Sleep(50);
            }
        }

        public void Dispose()
        {
            ComFinalizer.FinalRelease(_powerPointApplication);
            _powerPointApplication = null;
        }
    }
}
