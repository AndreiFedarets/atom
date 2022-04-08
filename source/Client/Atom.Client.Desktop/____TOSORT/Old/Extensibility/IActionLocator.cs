using System.Collections.Generic;

namespace Atom.Extensibility
{
    internal interface IActionLocator
    {
        IEnumerable<IActionType> LoadActions(IActionAssembly assembly);

        IEnumerable<IActionAssembly> LoadAssemblies(IDirectories directories);
    }
}
