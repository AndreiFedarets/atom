using System;

namespace Atom.Design.Reflection.Metadata
{
    [Serializable]
    public sealed class ParameterReference : IEquatable<ParameterReference>
    {
        public ParameterReference(string name, ParameterDirection direction, TypeReference parameterType)
        {
            Name = name;
            Direction = direction;
            ParameterType = parameterType;
        }

        public string Name { get; private set; }

        public TypeReference ParameterType { get; private set; }

        public ParameterDirection Direction { get; private set; }

        public bool Equals(ParameterReference other)
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
                   ParameterType.Equals(other.ParameterType) &&
                   Direction == other.Direction;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ParameterReference);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Name.GetHashCode();
                hash = hash * 23 + ParameterType.GetHashCode();
                hash = hash * 23 + Direction.GetHashCode();
                return hash;
            }
        }
    }
}
