using System;

namespace Atom.Runtime
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class ConditionMethodAttribute : Attribute
    {
        public ConditionMethodAttribute(string title)
        {
            Title = title;
        }

        public string Title { get; private set; }
    }
}
