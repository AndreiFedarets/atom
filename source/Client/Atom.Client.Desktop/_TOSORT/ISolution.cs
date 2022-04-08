using System;

namespace Atom.Design
{
    public interface ISolution
    {
        string Name { get; set; }

        IProjectCollection Projects { get; }

        event EventHandler<NameChangedEventArgs> NameChanged;
    }
}
