using Atom.Office.Win32;
using System;
using System.Diagnostics;
using System.Windows.Automation;

namespace Atom.Office.Excel
{
    internal class ExcelApplication : IExcelApplication
    {
        private Microsoft.Office.Interop.Excel._Application _excelApplication;
        private readonly ApplicationVersion _version;

        public const string MainWindowClassName = "XLMAIN";
        public const string DocumentWindowClassName = "EXCEL7";

        public ExcelApplication(Process excelProcess, ApplicationVersion version)
        {
            Process = excelProcess;
            _version = version;
        }

        public bool Visible
        {
            get
            {
                Initialize();
                return _excelApplication.Visible;
            }
        }

        public ApplicationType Type
        {
            get
            {
                Initialize();
                return ApplicationType.Excel;
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
                return new IntPtr(_excelApplication.Hwnd);
            }
        }

        public bool IsStarted
        {
            get
            {
                Process.Refresh();
                if (Process.HasExited)
                {
                    return false;
                }
                IntPtr mainWindowHandle = Process.GetProcessById(Process.Id).MainWindowHandle;
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

        public bool IsValid
        {
            get { return _excelApplication != null; }
        }

        public Process Process { get; private set; }

        public AutomationElement Element
        {
            get { return AutomationElement.FromHandle(WindowHandle); }
        }

        public void Initialize()
        {
            if (!IsValid)
            {
                _excelApplication = ApplicationProvider.FindExcelApplication(Process.GetProcessById(Process.Id).MainWindowHandle);
                if (_excelApplication == null)
                {
                    //throw new Exception();
                }
            }
        }

        public void Activate()
        {
            Initialize();
            NativeMethods.BringWindowToTop(WindowHandle);
        }

        public void Close()
        {
            _excelApplication.Quit();
            Dispose();
        }

        public void Dispose()
        {
            ComFinalizer.FinalRelease(ref _excelApplication);
        }
    }
}
