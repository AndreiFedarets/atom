using System;

namespace Interop.Native.Accessible
{
    public class StringProperty : AccessiblePropertyBase<string>
    {
        public StringProperty(Func<string> valueExtractor, bool cache, string defaultValue)
            : base(valueExtractor, cache, defaultValue)
        {
        }
    }
}
