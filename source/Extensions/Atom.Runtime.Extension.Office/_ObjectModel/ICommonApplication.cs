using System;
using System.Diagnostics;

namespace Atom.Office
{
    public interface ICommonApplication : IDisposable
    {
        bool Visible { get; }

        ApplicationType Type { get; }

        ApplicationVersion Version { get; }

        IntPtr WindowHandle { get; }

        Process Process { get; }

        bool IsStarted { get; }

        bool IsValid { get; }

        void Activate();

        void Close();

        void Initialize();
    }
}
