using Atom.Office.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Atom.Office.Excel
{
    public class ApplicationProvider : IApplicationProvider
    {
        private const double ProcessIdleCpuMaxUsage = 5;

        public int Count
        {
            get
            {
                string process = ApplicationTypeToProcessNameConverter.Convert(ApplicationType.Excel);
                return Process.GetProcessesByName(process).Length;
            }
        }

        public IExcelApplication Start(ApplicationVersion version)
        {
            Process process;
            IExcelApplication application;
            try
            {
                process = RunInternal(version);
                application = new ExcelApplication(process, version);
            }
            catch (Exception exception)
            {
                throw new OfficeApplicationRunException(ApplicationType.Excel, version, exception);
            }
            WaitForStarting(application);
            application.Initialize();
            return application;
        }

        public IExcelApplication Start()
        {
            throw new NotImplementedException();
        }

        private void WaitForStarting(IExcelApplication application)
        {
            application.Process.Refresh();
            if (application.Process.HasExited)
            {
                return;
            }
            while (NativeMethods.GetCpuUsage(application.Process) > ProcessIdleCpuMaxUsage)
            {
                Thread.Sleep(300);
            }
        }

        //public IExcelApplication Run(ApplicationVersion version)
        //{
        //    Process process = RunInternal(version);
        //    return new ExcelApplication(process, version);
        //}

        public void Close(IExcelApplication application)
        {
            application.Close();
            WaitForClosing(application);
        }

        private void WaitForClosing(IExcelApplication application)
        {
            application.Process.Refresh();
            while (!application.Process.HasExited)
            {
                Thread.Sleep(300);
                application.Process.Refresh();
            }
        }

        private Process RunInternal(ApplicationVersion version)
        {
            string applicationFullName = OfficePathHelper.GetApplicationFullPath(version, ApplicationType.Excel);
            ProcessStartInfo info = new ProcessStartInfo(applicationFullName);
            return Process.Start(info);
        }

        public static Microsoft.Office.Interop.Excel._Application FindExcelApplication(IntPtr mainWindowHandle)
        {
            Microsoft.Office.Interop.Excel.Window window = NativeMethods.FindWindowObject<Microsoft.Office.Interop.Excel.Window>(mainWindowHandle, ExcelApplication.DocumentWindowClassName);
            if (window != null)
            {
                Microsoft.Office.Interop.Excel._Application application = window.Application;
                ComFinalizer.Release(window);
                return application;
            }
            return null;
        }

        public IEnumerator<IExcelApplication> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
