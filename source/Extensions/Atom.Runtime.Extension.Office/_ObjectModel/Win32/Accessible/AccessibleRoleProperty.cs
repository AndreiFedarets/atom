using System;
using System.Windows.Forms;

namespace Interop.Native.Accessible
{
    public class AccessibleRoleProperty : AccessiblePropertyBase<AccessibleRole>
    {
        public AccessibleRoleProperty(Func<AccessibleRole> valueExtractor, bool cache, AccessibleRole defaultValue)
            : base(valueExtractor, cache, defaultValue)
        {
        }
    }
}
