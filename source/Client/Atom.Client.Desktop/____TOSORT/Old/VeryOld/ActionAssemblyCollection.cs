using System;
using System.Collections.Generic;
using Atom.Extensibility;

namespace Atom
{
    internal sealed class ActionAssemblyCollection : ReadOnlyDictionary<Guid, IActionAssembly>, IActionAssemblyCollection
    {
        internal void Load(IDirectories directories, IActionLocator actionLocator)
        {
            IEnumerable<IActionAssembly> assemblies = actionLocator.LoadAssemblies(directories);
            Items.Clear();
            foreach (IActionAssembly assembly in assemblies)
            {
                Items.Add(assembly.Uid, assembly);
            }
        }
    }
}
