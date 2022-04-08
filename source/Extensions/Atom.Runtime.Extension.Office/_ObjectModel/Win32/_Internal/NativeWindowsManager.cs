using Accessibility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Atom.Office.Win32
{
    internal static class NativeWindowsManager
    {
        public static IntPtr GetChild(IntPtr mainWindowHandle, string childWindowClassName)
        {
            if (mainWindowHandle != IntPtr.Zero)
            {
                var childHandle = IntPtr.Zero;

                var enumChildren = new NativeMethods.EnumWindowsProc((IntPtr currentChildHandle, ref IntPtr lParam) =>
                {
                    var buffer = new StringBuilder(128);
                    NativeMethods.GetClassName(currentChildHandle, buffer, 128);
                    if (buffer.ToString() == childWindowClassName)
                    {
                        lParam = currentChildHandle;
                        return false;
                    }
                    return true;
                });

                NativeMethods.EnumChildWindows(mainWindowHandle, enumChildren, ref childHandle);

                return childHandle;
            }
            return IntPtr.Zero;
        }

        public static IEnumerable<IAccessible> CollectTopLevelVisibleWindows()
        {
            IEnumerable<IntPtr> windowsHandles = NativeMethods.GetTopLevelWindows();
            IList<IAccessible> accessibles = new List<IAccessible>();
            foreach (IntPtr windowHandle in windowsHandles)
            {
                if (NativeMethods.IsWindowVisible(windowHandle))
                {
                    object result = null;
                    if (NativeMethods.AccessibleObjectFromWindow(windowHandle, Constants.OBJID_WINDOW, new Guid(Constants.IID_IAccessible).ToByteArray(), ref result) >= 0)
                    {
                        IAccessible accessible = result as IAccessible;
                        if (accessible != null)
                        {
                            accessibles.Add(accessible);
                        }
                    }
                }
            }
            return accessibles;
        }
    }
}
