using System;
using System.Runtime.InteropServices;

namespace Atom.Client.VisualStudio
{
    internal static class Releaser
    {
        public static void Release<T>(ref T obj) where T : class
        {
            if (obj == null)
            {
                return;
            }
            if (typeof(T).IsArray)
            {
                Array array = (Array)(object)obj;
                foreach (object arrayItem in array)
                {
                    object temp = arrayItem;
                    Release(ref temp);
                }
            }
            else if (Marshal.IsComObject(obj))
            {
                Marshal.ReleaseComObject(obj);
            }
            else if (obj is IDisposable)
            {
                ((IDisposable)obj).Dispose();
            }
            obj = null;
        }
    }
}
