using System;

namespace Atom.Design.Reflection.Metadata
{
    [Serializable]
    public sealed class PropertyReference : IEquatable<PropertyReference>
    {
        public PropertyReference(string name, TypeReference propertyType, TypeReference declaringType)
        {
            Name = name;
            PropertyType = propertyType;
            DeclaringType = declaringType;
        }

        public string Name { get; private set; }

        public TypeReference PropertyType { get; private set; }

        public TypeReference DeclaringType { get; private set; }

        public string FullName
        {
            get { return string.Concat(DeclaringType.FullName, ".", Name); }
        }

        public bool Equals(PropertyReference other)
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
                   PropertyType.Equals(other.PropertyType) && 
                   DeclaringType.Equals(other.DeclaringType);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PropertyReference);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Name.GetHashCode();
                hash = hash * 23 + PropertyType.GetHashCode();
                hash = hash * 23 + DeclaringType.GetHashCode();
                return hash;
            }
        }
    }
}
