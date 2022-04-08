using Accessibility;
using Interop.Native;
using Interop.Native.Accessible;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Interop.Office
{
    public class MenuCollection : IAccessibleObjectCollection
    {
        public const string CommandBarClassName = "MsoCommandBar";
        private readonly IntPtr _windowHandle;

        public MenuCollection(IntPtr windowHandle)
        {
            _windowHandle = windowHandle;
        }

        public int Count
        {
            get { return GetChildren().Count; }
        }

        public IAccessibleObject this[int index]
        {
            get { return GetChildren()[index]; }
        }

        public IAccessibleObject this[string name]
        {
            get { return GetChildren().FirstOrDefault(item => item.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)); }
        }

        public IEnumerator<IAccessibleObject> GetEnumerator()
        {
            return GetChildren().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private IList<IAccessibleObject> GetChildren()
        {
            IEnumerable<IAccessible> accessibles = NativeMethods.GetAllChilds(_windowHandle, CommandBarClassName);
            return accessibles.Select(accessible => new AccessibleObject(accessible)).Cast<IAccessibleObject>().ToList();
        }

        public void Refresh()
        {

        }

        public IAccessibleObject FindByName(string name, bool recursive)
        {
            foreach (IAccessibleObject accessibleObject in this)
            {
                if (string.Equals(accessibleObject.Name, name, StringComparison.InvariantCulture))
                {
                    return accessibleObject;
                }
                if (recursive)
                {
                    IAccessibleObject targetObject = accessibleObject.FindByName(name, recursive);
                    if (targetObject != null)
                    {
                        return targetObject;
                    }
                }
            }
            return null;
        }
    }
}
