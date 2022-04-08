using Accessibility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Atom.Office.Win32
{
    internal static class NativeMethods
    {
        [DllImport("user32.dll")]
        public static extern bool UpdateWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool InvalidateRect(IntPtr hWnd, IntPtr rect, bool bErase);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        public static extern void ReleaseDC(IntPtr dc);

        [DllImport("oleacc.dll")]
        public static extern int AccessibleObjectFromWindow(IntPtr hwnd, uint objectId, byte[] riid, [MarshalAs(UnmanagedType.IDispatch)] ref object @object);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr windowHandle);

        public delegate bool EnumWindowsProc(IntPtr windowHandle, ref IntPtr param);

        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumChildWindows(IntPtr parentHandle, EnumWindowsProc enumFunc, ref IntPtr param);

        [DllImport("User32.dll")]
        public static extern int GetClassName(IntPtr windowHandle, StringBuilder className, int maxCount);

        public static IntPtr GetFirstChild(IntPtr parentHandle, string childWindowClassName)
        {
            if (parentHandle != IntPtr.Zero)
            {
                IntPtr childHandle = IntPtr.Zero;
                EnumWindowsProc enumChildren = (IntPtr currentChildHandle, ref IntPtr param) =>
                {
                    var buffer = new StringBuilder(128);
                    GetClassName(currentChildHandle, buffer, 128);
                    if (string.Equals(buffer.ToString(), childWindowClassName, StringComparison.InvariantCulture))
                    {
                        param = currentChildHandle;
                        return false;
                    }
                    return true;
                };
                EnumChildWindows(parentHandle, enumChildren, ref childHandle);
                return childHandle;
            }
            return IntPtr.Zero;
        }

        public static IntPtr GetFirstChild(IntPtr parentHandle, string childWindowClassName, string childWindowCaption)
        {
            if (parentHandle != IntPtr.Zero)
            {
                IntPtr childHandle = IntPtr.Zero;
                EnumWindowsProc enumChildren = (IntPtr currentChildHandle, ref IntPtr param) =>
                {
                    var className = new StringBuilder(128);
                    GetClassName(currentChildHandle, className, 128);
                    if (className.ToString().Equals(childWindowClassName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        var caption = new StringBuilder(128);
                        GetWindowText(currentChildHandle, caption, 128);
                        if (caption.ToString().Equals(childWindowCaption, StringComparison.InvariantCultureIgnoreCase))
                        {
                            param = currentChildHandle;
                            return false;
                        }
                    }
                    return true;
                };
                EnumChildWindows(parentHandle, enumChildren, ref childHandle);
                return childHandle;
            }
            return IntPtr.Zero;
        }

        public static IEnumerable<IAccessible> GetAllChilds(IntPtr windowHandle, string className)
        {
            IList<IAccessible> accessibles = new List<IAccessible>();
            EnumWindowsProc enumChildren = (IntPtr currentChildHandle, ref IntPtr param) =>
            {
                var childClassName = new StringBuilder(128);
                GetClassName(currentChildHandle, childClassName, 128);
                if (childClassName.ToString().Equals(className, StringComparison.InvariantCultureIgnoreCase))
                {
                    object result = null;
                    if (AccessibleObjectFromWindow(currentChildHandle, Constants.OBJID_NATIVEOM, new Guid(Constants.IID_IDispatch).ToByteArray(), ref result) >= 0)
                    {
                        IAccessible accessible = result as IAccessible;
                        if (accessible != null)
                        {
                            accessibles.Add(accessible);
                        }
                    }
                }
                return true;
            };
            IntPtr zero = new IntPtr();
            EnumChildWindows(windowHandle, enumChildren, ref zero);
            return accessibles;
        }

        public static IEnumerable<IntPtr> GetTopLevelWindows()
        {
            IList<IntPtr> windowsHandles = new List<IntPtr>();
            EnumWindowsProc enumChildren = (IntPtr currentChildHandle, ref IntPtr param) =>
            {
                windowsHandles.Add(currentChildHandle);
                return true;
            };
            IntPtr zero = new IntPtr();
            EnumChildWindows(IntPtr.Zero, enumChildren, ref zero);
            return windowsHandles;
        }

        public static object FindAcessibleObject(IntPtr mainWindowHandle, string className, string caption)
        {
            IntPtr childHandle = GetFirstChild(mainWindowHandle, className, caption);
            if (childHandle != IntPtr.Zero)
            {
                object result = null;
                if (AccessibleObjectFromWindow(childHandle, Constants.OBJID_NATIVEOM, new Guid(Constants.IID_IDispatch).ToByteArray(), ref result) >= 0)
                {
                    return result;
                }
            }
            return null;
        }

        public static object FindAcessibleObject(IntPtr mainWindowHandle, string className)
        {
            IntPtr childHandle = GetFirstChild(mainWindowHandle, className);
            if (childHandle != IntPtr.Zero)
            {
                object result = null;
                if (AccessibleObjectFromWindow(childHandle, Constants.OBJID_NATIVEOM, new Guid(Constants.IID_IDispatch).ToByteArray(), ref result) >= 0)
                {
                    return result;
                }
            }
            return null;
        }

        public static T FindWindowObject<T>(IntPtr mainWindowHandle, string className)
        {
            object @object = FindAcessibleObject(mainWindowHandle, className);
            if (@object != null)
            {
                return (T)@object;
            }
            return default(T);
        }

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string className, string windowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool EnumChildWindows(IntPtr hWndParent, EnumWindowsProc lpEnumFunc, int lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetWindowText(IntPtr hwnd, StringBuilder lpString, int cch);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetWindowTextLength(IntPtr hwnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool BringWindowToTop(IntPtr windowHandle);

        [DllImport("oleacc.dll")]
        public static extern uint GetRoleText(uint dwRole, [Out] StringBuilder lpszRole, uint cchRoleMax);

        [DllImport("oleacc.dll")]
        public static extern uint GetStateText(uint dwStateBit, [Out] StringBuilder lpszStateBit, uint cchStateBitMax);

        [DllImport("oleacc.dll")]
        public static extern uint WindowFromAccessibleObject(IAccessible pacc, ref IntPtr phwnd);

        [DllImport("oleacc.dll", PreserveSig = false)]
        [return: MarshalAs(UnmanagedType.Interface)]
        public static extern object AccessibleObjectFromWindow(int hwnd, int dwId, ref Guid riid);

        [DllImport("oleacc.dll")]
        public static extern int AccessibleChildren(IAccessible paccContainer, int iChildStart, int cChildren,
                                                     [Out] [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] object[] rgvarChildren, ref int pcObtained);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetWindowRect(IntPtr handle, out Rect rectangle);

        public static double GetCpuUsage(Process process)
        {
            string name = string.Empty;
            foreach (var instance in new PerformanceCounterCategory("Process").GetInstanceNames())
            {
                if (instance.StartsWith(process.ProcessName))
                {
                    using (PerformanceCounter processId = new PerformanceCounter("Process", "ID Process", instance, true))
                    {
                        if (process.Id == (int)processId.RawValue)
                        {
                            name = instance;
                            break;
                        }
                    }
                }
            }
            PerformanceCounter processorTimeCounter = new PerformanceCounter("Process", "% Processor Time", name, true);
            processorTimeCounter.NextValue();
            Thread.Sleep(100);
            double cpuUsage = Math.Round(processorTimeCounter.NextValue() / System.Environment.ProcessorCount, 2);
            return cpuUsage;
        }
    }
}
