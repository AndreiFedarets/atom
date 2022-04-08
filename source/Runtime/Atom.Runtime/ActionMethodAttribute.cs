using System;

namespace Atom.Runtime
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class ActionMethodAttribute : Attribute
    {
        public ActionMethodAttribute(string title)
        {
            Title = title;
        }

        public string Title { get; private set; }
    }
}
