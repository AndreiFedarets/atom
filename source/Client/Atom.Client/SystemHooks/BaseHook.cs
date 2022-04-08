using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Atom.Client.SystemHooks
{
    public abstract class BaseHook : IDisposable
    {
        private delegate int HookCallback(int code, IntPtr wParam, IntPtr lParam);

        private HookCallback _handler;
        private readonly int _idHook;
        private IntPtr _hookHandle;
        private bool _disposed;

        protected BaseHook(int idHook)
        {
            _idHook = idHook;
            _handler = new HookCallback(OnEventInternal);
        }

        public void Subscribe()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().FullName);
            }
            using (Process currentProcess = Process.GetCurrentProcess())
            {
                using (ProcessModule currentModule = currentProcess.MainModule)
                {
                    IntPtr moduleHandle = GetModuleHandle(currentModule.ModuleName);
                    _hookHandle = SetWindowsHookEx(_idHook, _handler, moduleHandle, 0);
                }
            }
        }

        public void Unsubscribe()
        {
            if (_hookHandle != IntPtr.Zero && UnhookWindowsHookEx(_hookHandle))
            {
                _hookHandle = IntPtr.Zero;
            }
        }

        public void Dispose()
        {
            Unsubscribe();
            _disposed = true;
        }

        private int OnEventInternal(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code >= 0 && OnEvent(code, wParam, lParam))
            {
                return 1;
            }
            return CallNextHookEx(_hookHandle, code, wParam, lParam);
        }

        protected abstract bool OnEvent(int code, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookCallback lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        private static extern int CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
