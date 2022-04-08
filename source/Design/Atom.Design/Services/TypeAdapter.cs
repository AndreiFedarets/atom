using Atom.Design.Reflection.Metadata;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Atom.Design.Services
{
    public abstract class TypeAdapter : IEquatable<TypeAdapter>
    {
        public TypeAdapter(TypeReference systemType)
        {
            SystemType = systemType;
        }

        public TypeReference SystemType { get; }

        public abstract object ReadValue(XElement parentElement);

        public abstract void WriteValue(XElement parentElement, object value);

        public abstract IEnumerable<CodeStatement> GenerateCode(object value);

        public bool Equals(TypeAdapter other)
        {
            if (other == null)
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return SystemType.Equals(other.SystemType);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TypeAdapter);
        }

        public override int GetHashCode()
        {
            return SystemType.GetHashCode();
        }
    }
}
