using System;
using System.IO;
using System.Reflection;
using System.Timers;

namespace Atom.Design.Reflection.Binary
{
    internal class SandboxAppDomain : IDisposable
    {
        private const int DefaultKeepAlive = 10;
        private static readonly object AccessLock;
        private static readonly Timer Timer;
        private static AppDomain AppDomain;
        private static int ReferenceCount;

        static SandboxAppDomain()
        {
            AccessLock = new object();
            Timer = new Timer();
            Timer.AutoReset = false;
            Timer.Elapsed += OnTimerElapsed;
            Timer.Interval = TimeSpan.FromSeconds(DefaultKeepAlive).TotalMilliseconds;
        }

        public SandboxAppDomain()
        {
            Initialize();
        }

        void IDisposable.Dispose()
        {
            int references = System.Threading.Interlocked.Decrement(ref ReferenceCount);
            if (references == 0)
            {
                Timer.Start();
            }
        }

        public T CreateInstance<T>(params object[] args)
        {
            Type type = typeof(T);
            return (T)AppDomain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName, false, BindingFlags.CreateInstance, null, args, null, null);
        }

        private void Initialize()
        {
            System.Threading.Interlocked.Increment(ref ReferenceCount);
            Timer.Stop();
            lock (AccessLock)
            {
                if (AppDomain == null)
                {
                    AppDomain = CreateAppDomain();
                }
            }
        }

        private static AppDomain CreateAppDomain()
        {
            const bool shadowCopyFiles = false;
            string friendlyName = typeof(SandboxAppDomain).FullName;
            AppDomainSetup appDomainSetup = new AppDomainSetup();
            appDomainSetup.ShadowCopyFiles = shadowCopyFiles.ToString();
            appDomainSetup.ApplicationBase = GetAppDomainBaseDirectory();
            AppDomain appDomain = AppDomain.CreateDomain(friendlyName, null, appDomainSetup);
            return appDomain;
        }

        private static string GetAppDomainBaseDirectory()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            string path = assembly.Location;
            path = Path.GetDirectoryName(path);
            return path;
        }

        private static void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            lock (AccessLock)
            {
                if (ReferenceCount == 0)
                {
                    AppDomain.Unload(AppDomain);
                    AppDomain = null;
                }
            }
        }
    }
}
