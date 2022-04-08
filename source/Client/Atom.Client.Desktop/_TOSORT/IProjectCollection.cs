using System;
using System.Collections.Generic;

namespace Atom.Design
{
    public interface IProjectCollection : IReadOnlyCollection<IProject>
    {
        IProject Create(string name);

        event EventHandler<ProjectEventArgs> ProjectAdded;

        event EventHandler<ProjectEventArgs> ProjectRemoved;
    }
}
