using System;

namespace Atom.Design.Reflection.Metadata
{
    [Serializable]
    public sealed class MethodReference : IEquatable<MethodReference>
    {
        public MethodReference(string name, ParameterReferenceCollection parameters, TypeReference declaringType)
        {
            Name = name;
            Parameters = parameters;
            DeclaringType = declaringType;
        }

        public string Name { get; private set; }

        public ParameterReferenceCollection Parameters { get; private set; }

        public TypeReference DeclaringType { get; private set; }

        public string FullName
        {
            get { return string.Concat(DeclaringType.FullName, ".", Name); }
        }

        public bool Equals(MethodReference other)
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
                   DeclaringType.Equals(other.DeclaringType) &&
                   Parameters.Equals(other.Parameters);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as MethodReference);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Name.GetHashCode();
                hash = hash * 23 + DeclaringType.GetHashCode();
                hash = hash * 23 + Parameters.GetHashCode();
                return hash;
            }
        }
    }
}
