using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Atom.Design.Reflection.Metadata
{
    [Serializable]
    public sealed class ParameterReferenceCollection : ReadOnlyCollection<ParameterReference>, IEquatable<ParameterReferenceCollection>
    {
        public ParameterReferenceCollection(List<ParameterReference> collection)
            : base(collection)
        {
        }

        public ParameterReference this[string name]
        {
            get
            {
                ParameterReference parameter = Items.First(x => string.Equals(x.Name, name, StringComparison.Ordinal));
                return parameter;
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ParameterReferenceCollection);
        }

        public bool Equals(ParameterReferenceCollection other)
        {
            if (other == null)
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            if (Count != other.Count)
            {
                return false;
            }
            for (int i = 0; i < Count; i++)
            {
                ParameterReference thisParameter = this[i];
                ParameterReference otherParameter = other[i];
                if (!thisParameter.Equals(otherParameter))
                {
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                foreach (ParameterReference parameter in Items)
                {
                    hash = hash * 23 + parameter.GetHashCode();
                }
                return hash;
            }
        }
    }
}
