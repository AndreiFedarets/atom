using System;
using System.Collections.Generic;

namespace Atom.Design.Hosting
{
    public interface IProjectCollection : IEnumerable<IProject>
    {
        event EventHandler ProjectAdded;

        event EventHandler ProjectRemoved;
    }
}
