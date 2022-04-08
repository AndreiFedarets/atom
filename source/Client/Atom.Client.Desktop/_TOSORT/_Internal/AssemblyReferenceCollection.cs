using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Atom.Design
{
    internal sealed class AssemblyReferenceCollection : ReadOnlyCollection<IAssemblyReference>, IAssemblyReferenceCollection
    {
        public AssemblyReferenceCollection()
            : base(new List<IAssemblyReference>())
        {
        }

        public event EventHandler<AssemblyReferenceEventArgs> ReferencesAdded;

        public event EventHandler<AssemblyReferenceEventArgs> ReferencesRemoved;

        public IAssemblyReference Add(IAssembly assembly)
        {
            IAssemblyReference reference = new AssemblyReference(assembly.Metadata, false);
            Items.Add(reference);
            AssemblyReferenceEventArgs.RaiseEvent(ReferencesAdded, this, reference);
            return reference;
        }

        public bool Remove(IAssemblyReference reference)
        {
            bool result = Items.Remove(reference);
            if (result)
            {
                AssemblyReferenceEventArgs.RaiseEvent(ReferencesRemoved, this, reference);
            }
            return result;
        }
    }
}
