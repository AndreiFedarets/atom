using Accessibility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Interop.Native.Accessible
{
    public class AccessibleObjectCollection : IAccessibleObjectCollection
    {
        private readonly IAccessible _accessible;
        private IList<IAccessibleObject> _children;

        public AccessibleObjectCollection(IAccessible accessible)
        {
            _accessible = accessible;
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
            get { return GetChildren().FirstOrDefault(item => item.Name.Equals(name, StringComparison.InvariantCulture)); }
        }

        public IEnumerator<IAccessibleObject> GetEnumerator()
        {
            return GetChildren().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public virtual void Refresh()
        {
            _children = null;
        }

        private IList<IAccessibleObject> GetChildren()
        {
            if (_children == null)
            {
                int childCount = 0;
                try
                {
                    childCount = _accessible.accChildCount;
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.ToString());
                }
                object[] accObjects = new object[childCount];
                int count = 0;
                if (childCount != 0)
                {
                    NativeMethods.AccessibleChildren(_accessible, 0, childCount, accObjects, ref count);
                }
                _children = accObjects.Where(x => x != null && x.GetType() != typeof(int)).Select(x => new AccessibleObject((IAccessible)x)).Cast<IAccessibleObject>().ToList();
            }
            return _children;
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
