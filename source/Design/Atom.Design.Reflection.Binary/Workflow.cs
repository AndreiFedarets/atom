using Atom.Design.Reflection.Metadata;
using System;

namespace Atom.Design.Reflection.Binary
{
    [Serializable]
    public sealed class Workflow : IWorkflow, IEquatable<Workflow>
    {
        public Workflow(string title, MethodReference method)
        {
            Title = title;
            Reference = method;
        }

        public string Title { get; private set; }

        public MethodReference Reference { get; private set; }

        public bool Equals(Workflow other)
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
            return Equals(obj as Workflow);
        }

        public override int GetHashCode()
        {
            return Reference.GetHashCode();
        }
    }
}
