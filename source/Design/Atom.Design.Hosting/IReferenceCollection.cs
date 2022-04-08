using System;
using System.Collections.Generic;

namespace Atom.Design.Hosting
{
    public interface IReferenceCollection : IEnumerable<IReference>
    {
        event EventHandler ReferenceAdded;

        event EventHandler ReferenceRemoved;
    }
}
