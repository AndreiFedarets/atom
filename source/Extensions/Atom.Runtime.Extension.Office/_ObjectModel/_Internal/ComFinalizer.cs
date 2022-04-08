using System;
using System.Runtime.InteropServices;

namespace Atom.Office
{
    internal static class ComFinalizer
    {
        public static void Release(object @object)
        {
            try
            {
                Marshal.ReleaseComObject(@object);
            }
            catch (Exception)
            {
               
            }
        }

        public static void Release<T>(ref T @object)
        {
            try
            {
                Marshal.ReleaseComObject(@object);
                @object = default(T);
            }
            catch (Exception)
            {

            }
        }

        public static void FinalRelease<T>(ref T @object)
        {
            try
            {
                Marshal.FinalReleaseComObject(@object);
                @object = default(T);
            }
            catch (Exception)
            {
                
            }
        }
    }
}
