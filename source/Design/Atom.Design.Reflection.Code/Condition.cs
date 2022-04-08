using Atom.Design.Reflection.Metadata;
using System;

namespace Atom.Design.Reflection.Code
{
    public sealed class Condition : ICondition, IEquatable<Condition>
    {
        public Condition(string title, MethodReference method)
        {
            Title = title;
            Reference = method;
        }

        public string Title { get; private set; }

        public MethodReference Reference { get; private set; }

        public bool Equals(Condition other)
        {
            if (other == null)
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Reference.Equals(other.Reference);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Condition);
        }

        public override int GetHashCode()
        {
            return Reference.GetHashCode();
        }
    }
}
