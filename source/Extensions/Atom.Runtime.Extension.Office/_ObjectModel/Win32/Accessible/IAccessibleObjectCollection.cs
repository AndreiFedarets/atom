using System.Collections.Generic;

namespace Interop.Native.Accessible
{
    public interface IAccessibleObjectCollection : IEnumerable<IAccessibleObject>
    {
        int Count { get; }

        void Refresh();

        IAccessibleObject this[int index] { get; }

        IAccessibleObject this[string name] { get; }

        IAccessibleObject FindByName(string name, bool recursive);
    }
}
