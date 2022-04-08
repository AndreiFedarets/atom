using System;

namespace Atom.Runtime
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class DataTableAttribute : Attribute
    {
        public DataTableAttribute(string title)
        {
            Title = title;
        }

        public string Title { get; private set; }
    }
}
