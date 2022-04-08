using System;

namespace Atom.Runtime
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class ActionParameterAttribute : Attribute
    {
        public ActionParameterAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}
