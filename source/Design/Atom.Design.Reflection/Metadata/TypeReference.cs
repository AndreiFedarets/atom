using System;
using System.Collections.Generic;

namespace Atom.Design.Reflection.Metadata
{
    [Serializable]
    public sealed class TypeReference : IEquatable<TypeReference>
    {
        public TypeReference(string name, string @namespace, AssemblyReference assembly, params TypeReference[] baseTypes)
        {
            Name = name;
            Namespace = @namespace;
            Assembly = assembly;
            BaseTypes = baseTypes;
        }

        public string Name { get; private set; }

        public string Namespace { get; private set; }

        public string FullName
        {
            get { return string.Concat(Namespace, ".", Name); }
        }

        public IEnumerable<TypeReference> BaseTypes { get; private set; }

        public AssemblyReference Assembly { get; private set; }

        public bool Equals(TypeReference other)
        {
            if (other == null)
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return string.Equals(Name, other.Name, StringComparison.Ordinal) &&
                   string.Equals(Namespace, other.Namespace, StringComparison.Ordinal) &&
                   Assembly.Equals(other.Assembly);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj as TypeReference);
        }

        public override int GetHashCode()
        {
            return FullName.GetHashCode();
        }

        public bool IsAssignableFrom(TypeReference type)
        {
            if (Equals(type))
            {
                return true;
            }
            foreach (TypeReference baseType in type.BaseTypes)
            {
                if (IsAssignableFrom(baseType))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
