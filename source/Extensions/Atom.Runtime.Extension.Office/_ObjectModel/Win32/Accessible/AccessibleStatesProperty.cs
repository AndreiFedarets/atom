using System;
using System.Windows.Forms;

namespace Interop.Native.Accessible
{
    public class AccessibleStatesProperty : AccessiblePropertyBase<AccessibleStates>
    {
        public AccessibleStatesProperty(Func<AccessibleStates> valueExtractor, bool cache, AccessibleStates defaultValue)
            : base(valueExtractor, cache, defaultValue)
        {
        }
    }
}
