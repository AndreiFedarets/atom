using Atom.Office.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Atom.Office.Word
{
    public class ApplicationProvider : IApplicationProvider
    {
        public int Count
        {
            get
            {
                string process = ApplicationTypeToProcessNameConverter.Convert(ApplicationType.Word);
                return Process.GetProcessesByName(process).Length;
            }
        }

        public IWordApplication Create(ApplicationVersion version)
        {
            Process process;
            IWordApplication application;
            try
            {
                process = RunInternal(version);
                application = new WordApplication(process, version);
            }
            catch (Exception exception)
            {
                throw new OfficeApplicationRunException(ApplicationType.Word, version, exception);
            }
            WaitForStarting(application);
            application.Initialize();
            return application;
        }

        public void WaitForStarting(IWordApplication application)
        {
            do
            {
                Thread.Sleep(50);

            } while (!application.IsStarted);
        }

        public IWordApplication Run(ApplicationVersion version)
        {
            Process process = RunInternal(version);
            return new WordApplication(process, version);
        }

        private Process RunInternal(ApplicationVersion version)
        {
            string applicationFullName = OfficePathHelper.GetApplicationFullPath(version, ApplicationType.Word);
            ProcessStartInfo info = new ProcessStartInfo(applicationFullName);
            return Process.Start(info);
        }

        public static Microsoft.Office.Interop.Word._Application FindWordApplication(IntPtr mainWindowHandle)
        {
            Microsoft.Office.Interop.Word.Window window = NativeMethods.FindWindowObject<Microsoft.Office.Interop.Word.Window>(mainWindowHandle, WordApplication.DocumentWindowClassName);
            if (window != null)
            {
                Microsoft.Office.Interop.Word._Application application = window.Application;
                ComFinalizer.Release(window);
                return application;
            }
            return null;
        }

        public IEnumerator<IWordApplication> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
