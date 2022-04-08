using System;
using System.Drawing;
using System.Windows.Forms;

namespace Interop.Native.Accessible
{
    public interface IAccessibleObject : IAccessibleObjectCollection
    {
        string Name { get; }

        string ClassName { get; }

        IAccessibleObject Parent { get; }

        string DefaultAction { get; }

        string Description { get; }

        Rectangle Bounds { get; }

        IntPtr WindowHandle { get; }

        AccessibleRole Role { get; }

        AccessibleStates State { get; }

        string Value { get; }

        void Do();

        void Highlight();

        void Unhighlight();
    }
}
