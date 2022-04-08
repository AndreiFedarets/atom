using System;
using System.Reflection;

namespace Atom.Design.Reflection.Metadata
{
    [Serializable]
    public sealed class AssemblyReference : IEquatable<AssemblyReference>
    {
        public AssemblyReference(AssemblyName assemblyName)
        {
            Name = assemblyName.ToString();
        }

        public AssemblyReference(string assemblyName)
        {
            Name = assemblyName;
        }

        public string Name { get; private set; }

        public bool Equals(AssemblyReference other)
        {
            if (other == null)
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            //Ignore Version comparison
            return string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj as AssemblyReference);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
