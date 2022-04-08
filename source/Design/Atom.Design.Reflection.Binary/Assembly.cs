using Atom.Design.Reflection.Metadata;
using System;

namespace Atom.Design.Reflection.Binary
{
    [Serializable]
    public sealed class Assembly : IAssembly, IEquatable<Assembly>
    {
        public Assembly(AssemblyReference reference, IActionCollection actions, IConditionCollection conditions, ITableCollection tables)
        {
            Reference = reference;
            Actions = actions;
            Conditions = conditions;
            Tables = tables;
        }

        public AssemblyReference Reference { get; private set; }

        public IActionCollection Actions { get; private set; }

        public IConditionCollection Conditions { get; private set; }

        public ITableCollection Tables { get; private set; }

        public bool Equals(Assembly other)
        {
            if (other == null)
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Reference.Equals(Reference);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Assembly);
        }

        public override int GetHashCode()
        {
            return Reference.GetHashCode();
        }
    }
}
