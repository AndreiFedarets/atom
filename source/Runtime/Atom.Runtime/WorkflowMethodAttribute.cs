using System;

namespace Atom.Runtime
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class WorkflowMethodAttribute : Attribute
    {
        public WorkflowMethodAttribute(string title)
        {
            Title = title;
        }

        public string Title { get; private set; }
    }
}
