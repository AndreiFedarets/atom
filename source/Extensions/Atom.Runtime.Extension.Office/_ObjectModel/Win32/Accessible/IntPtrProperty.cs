using System;

namespace Interop.Native.Accessible
{
    public class IntPtrProperty : AccessiblePropertyBase<IntPtr>
    {
        public IntPtrProperty(Func<IntPtr> valueExtractor, bool cache, IntPtr defaultValue)
            : base(valueExtractor, cache, defaultValue)
        {
        }
    }
}
