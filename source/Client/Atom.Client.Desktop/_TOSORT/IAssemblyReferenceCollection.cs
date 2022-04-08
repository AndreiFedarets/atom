using System;
using System.Collections.Generic;

namespace Atom.Design
{
    public interface IAssemblyReferenceCollection : IReadOnlyCollection<IAssemblyReference>
    {
        event EventHandler<AssemblyReferenceEventArgs> ReferencesAdded;

        event EventHandler<AssemblyReferenceEventArgs> ReferencesRemoved;

        IAssemblyReference Add(IAssembly assembly);

        bool Remove(IAssemblyReference reference);
    }
}
