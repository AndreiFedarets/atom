using Atom.Design.Reflection.Metadata;
using System;

namespace Atom.Design.Reflection.Code
{
    public sealed class TableValue : ITableValue, IEquatable<TableValue>
    {
        public TableValue(PropertyReference property)
        {
            Property = property;
        }

        public PropertyReference Property { get; private set; }

        public string ValueName
        {
            get { return Property.Name; }
        }

        public TypeReference ValueType
        {
            get { return Property.PropertyType; }
        }

        public BaseValue CreateValue()
        {
            return new PropertyValue(Property);
        }

        public bool Equals(TableValue other)
        {
            if (other == null)
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Property.Equals(other.Property);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TableValue);
        }

        public override int GetHashCode()
        {
            return Property.GetHashCode();
        }
    }
}
