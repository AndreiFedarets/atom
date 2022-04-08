using Atom.Office.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Atom.Office.PowerPoint
{
    public class ApplicationProvider : IApplicationProvider
    {
        public int Count
        {
            get
            {
                string process = ApplicationTypeToProcessNameConverter.Convert(ApplicationType.PowerPoint);
                return Process.GetProcessesByName(process).Length;
            }
        }

        public IPowerPointApplication Create(ApplicationVersion version)
        {
            Process process;
            IPowerPointApplication application;
            try
            {
                process = RunInternal(version);
                application = new PowerPointApplication(process, version);
            }
            catch (Exception exception)
            {
                throw new OfficeApplicationRunException(ApplicationType.PowerPoint, version, exception);
            }
            WaitForStarting(application);
            application.Initialize();
            return application;
        }

        public void WaitForStarting(IPowerPointApplication application)
        {
            do
            {
                Thread.Sleep(50);

            } while (!application.IsStarted);
        }

        public IPowerPointApplication Run(ApplicationVersion version)
        {
            Process process = RunInternal(version);
            return new PowerPointApplication(process, version);
        }

        private Process RunInternal(ApplicationVersion version)
        {
            string applicationFullName = OfficePathHelper.GetApplicationFullPath(version, ApplicationType.PowerPoint);
            ProcessStartInfo info = new ProcessStartInfo(applicationFullName);
            return Process.Start(info);
        }

        public static Microsoft.Office.Interop.PowerPoint._Application FindPowerPointApplication(IntPtr mainWindowHandle)
        {
            Microsoft.Office.Interop.PowerPoint.DocumentWindow window = NativeMethods.FindWindowObject<Microsoft.Office.Interop.PowerPoint.DocumentWindow>(mainWindowHandle, PowerPointApplication.DocumentWindowClassName);
            if (window != null)
            {
                Microsoft.Office.Interop.PowerPoint._Application application = window.Application;
                ComFinalizer.Release(window);
                return application;
            }
            return null;
        }

        public IEnumerator<IPowerPointApplication> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
