using System;

namespace Interop.Native.Accessible
{
    public class AccessibleObjectProperty : AccessiblePropertyBase<IAccessibleObject>
    {
        public AccessibleObjectProperty(Func<IAccessibleObject> valueExtractor, bool cache, IAccessibleObject defaultValue)
            : base(valueExtractor, cache, defaultValue)
        {
        }
    }
}
