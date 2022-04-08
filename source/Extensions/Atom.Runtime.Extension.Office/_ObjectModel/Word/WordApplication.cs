using Atom.Office.Win32;
using System;
using System.Diagnostics;
using System.Threading;

namespace Atom.Office.Word
{
    internal class WordApplication : IWordApplication
    {
        private Microsoft.Office.Interop.Word._Application _wordApplication;
        private readonly ApplicationVersion _version;
        private readonly Process _wordProcess;

        public const string MainWindowClassName = "OpusApp";
        public const string DocumentWindowClassName = "_WwG";

        public WordApplication(Process wordProcess, ApplicationVersion version)
        {
            _wordProcess = wordProcess;
            _version = version;
        }

        public bool Visible
        {
            get
            {
                Initialize();
                return _wordApplication.Visible;
            }
        }

        public ApplicationType Type
        {
            get
            {
                Initialize();
                return ApplicationType.Word;
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
                return _wordProcess.MainWindowHandle;
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
                if (_wordProcess.HasExited)
                {
                    return false;
                }
                IntPtr mainWindowHandle = Process.GetProcessById(_wordProcess.Id).MainWindowHandle;
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
                _wordApplication = ApplicationProvider.FindWordApplication(Process.GetProcessById(_wordProcess.Id).MainWindowHandle);
            }
        }

        public bool IsValid
        {
            get
            {
                return _wordApplication != null;
            }
        }

        public void Close()
        {
            _wordApplication.Quit();
            Dispose();
            while (!_wordProcess.HasExited)
            {
                Thread.Sleep(50);
            }
        }

        public void Dispose()
        {
            ComFinalizer.FinalRelease(_wordApplication);
            _wordApplication = null;
        }
    }
}
