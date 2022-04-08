using System;

namespace Atom.Design
{
    public interface IProject
    {
        Guid Uid { get; }

        string Name { get; set; }

        IAssembly ShadowAssembly { get; }

        ISolution Solution { get; }

        ProjectSettings Settings { get; }

        IAssemblyReferenceCollection References { get; }

        IProjectFolder ProjectFolder { get; }

        event EventHandler<NameChangedEventArgs> NameChanged;
    }
}
